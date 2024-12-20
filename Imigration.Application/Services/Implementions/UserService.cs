﻿using Imigration.Application.Extensions;
using Imigration.Application.Generators;
using Imigration.Application.Security;
using Imigration.Application.Services.Interfaces;
using Imigration.Application.Statics;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Enums;
using Imigration.Domains.Interfaces;
using Imigration.Domains.ViewModels.Account;
using Imigration.Domains.ViewModels.Admin.User;
using Imigration.Domains.ViewModels.Common;
using Imigration.Domains.ViewModels.UserPanel.Account;
using Microsoft.Extensions.Options;

namespace Imigration.Application.Services.Implementions
{
    public class UserService : IUserService
    {
        #region Ctor
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private ScoreManagementViewModel _scoreManagement;
        public UserService(IUserRepository userRepository, IEmailService emailService, IOptions<ScoreManagementViewModel> scoreManagement)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _scoreManagement = scoreManagement.Value;
        }
        #endregion

        #region Register

        public async Task<RegisterResult> RegisterUser(RegisterViewModel register)
        {
            // Check Email Existsx
            if (await _userRepository.IsExistsUserByEmail(register.Email.SanitizeText().Trim().ToLower()))
            {
                return RegisterResult.EmailExists;
            }

            // hash password
            var password = PasswordHelper.EncodePasswordMd5(register.Password.SanitizeText());

            // create user
            var user = new User
            {
                Avatar = PathTools.DefaultUserAvatar,
                Email = register.Email.SanitizeText().Trim().ToLower(),
                Password = password,
                EmailActivationCode = CodeGenerator.CreateActivationCode()
            };

            // Add To database
            //await _userRepository.CreateUser(user);
            //await _userRepository.Save();

            

            //var body = $@"
            //    <div> برای فعالسازی حساب کاربری خود روی لینک زیر کلیک کنید . </div>
            //    <a href='{PathTools.SiteAddress}/Activate-Email/{user.EmailActivationCode}'>فعالسازی حساب کاربری</a>
            //    ";

            //await _emailService.SendEmail(user.Email, "فعالسازی حساب کاربری", body);

            

            return RegisterResult.Success;
        }

#endregion

        #region Login
        public async Task<LoginResult> CheckUserForLogin(LoginViewModel login)
        {
            var user = await _userRepository.GetUserByEmail(login.Email.Trim().ToLower().SanitizeText());

            if (user == null) return LoginResult.UserNotFound;

            var hashedPassword = PasswordHelper.EncodePasswordMd5(login.Password.SanitizeText());

            if (hashedPassword != user.Password)
            {
                return LoginResult.UserNotFound;
            }

            if (user.IsDelete) return LoginResult.UserNotFound;

            if (user.IsBan) return LoginResult.UserIsBan;

            if (!user.IsEmailConfirmed) return LoginResult.EmailNotActivated;

            return LoginResult.Success;

        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }


        #endregion
        //Robot Data structure new life
        #region Activation Email

        public async Task<bool> ActivateUserEmail(string activationCode)
        {
            var user = await _userRepository.GetUserByActivationCode(activationCode);

            if (user == null) return false;

            if (user.IsBan || user.IsDelete) return false;

            user.IsEmailConfirmed = true;
            user.EmailActivationCode = CodeGenerator.CreateActivationCode();
            await _userRepository.UpdateUser(user);
            await _userRepository.Save();

            return true;
        }



        #endregion

        #region ForgotPassword
        public async Task<ForgotPasswordResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            var email = forgotPassword.Email.SanitizeText().Trim().ToLower();

            var user = await _userRepository.GetUserByEmail(email);

            if (user == null || user.IsDelete) return ForgotPasswordResult.UserNotFound;

            if (user.IsBan) return ForgotPasswordResult.UserBan;

            #region Send Activation Email

            var body = $@"
                <div> برای فراموشی کلمه عبور روی لینک زیر کلیک کنید . </div>
                <a href='{PathTools.SiteAddress}/Reset-Password/{user.EmailActivationCode}'>فراموشی کلمه عبور</a>
                ";

            _emailService.SendEmail(user.Email, "فراموشی کلمه عبور", body);

            #endregion

            return ForgotPasswordResult.Success;
        }


        #endregion

        #region ResetPassword

        public async Task<ResetPasswordResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            var user = await _userRepository.GetUserByActivationCode(resetPassword.EmailActivationCode.SanitizeText());

            if (user == null || user.IsDelete) return ResetPasswordResult.UserNotFound;

            if (user.IsBan) return ResetPasswordResult.UserIsBan;

            var password = PasswordHelper.EncodePasswordMd5(resetPassword.Password.SanitizeText());

            user.Password = password;

            user.IsEmailConfirmed = true;
            user.EmailActivationCode = CodeGenerator.CreateActivationCode();

            await _userRepository.UpdateUser(user);
            await _userRepository.Save();

            return ResetPasswordResult.Success;


        }

        public async Task<User> GetUserByActivationCode(string activationCode)
        {
            return await _userRepository.GetUserByActivationCode(activationCode.SanitizeText());
        }


        #endregion

        #region User Panel
        public async Task<User?> GetUserById(long userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task ChangeUserAvatar(long userId, string fileName)
        {
            var user = await GetUserById(userId);

            #region Delete Avatar 

            if (user.Avatar != PathTools.DefaultUserAvatar)
            {
                user.Avatar.DeleteFile(PathTools.UserAvatarPath);
            }

            #endregion

            user.Avatar = fileName;
            await _userRepository.UpdateUser(user);
            await _userRepository.Save();

        }

        public async Task<EditUserViewModel> FillEditUserViewModel(long userId)
        {
            var user = await _userRepository.GetUserById(userId);

            var result = new EditUserViewModel()
            {
                BirthDate = user.BirthDate != null ? user.BirthDate.Value.ToShamsi() : string.Empty,
                CityId = user.CityId,
                CountryId = user.CountryId,
                Description = user.Description,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                GetNewsLetter = user.GetNewsLetter,
                PhoneNumber = user.PhoneNumber,
            };
            return result;

        }

        public async Task<EditUserInfoResult> EditUserInfo(EditUserViewModel editUserViewModel, long userId)
        {
            var user = await GetUserById(userId);

            if (!string.IsNullOrEmpty(editUserViewModel.BirthDate))
            {
                try
                {
                    var date = editUserViewModel.BirthDate.SanitizeText().ToMiladi();
                    user.BirthDate = date;
                }
                catch (Exception exeption)
                {
                    return EditUserInfoResult.NotValidDate;

                }
            }

            user.FirstName = editUserViewModel.FirstName;
            user.LastName = editUserViewModel.LastName;
            user.PhoneNumber = editUserViewModel.PhoneNumber.SanitizeText();
            user.Description = editUserViewModel.Description.SanitizeText();
            user.GetNewsLetter = editUserViewModel.GetNewsLetter;
            user.CountryId = editUserViewModel.CountryId;
            user.CityId = editUserViewModel.CityId;


            await _userRepository.UpdateUser(user);
            await _userRepository.Save();

            return EditUserInfoResult.Success;
        }

        public async Task<ChangeUserPasswordResult> ChangeUserPassword(long userId, ChangeUserPasswordViewModel changeUserPassword)
        {
            var user = await GetUserById(userId);

            var password = PasswordHelper.EncodePasswordMd5(changeUserPassword.OldPassword.SanitizeText());

            if (password != user.Password)
            {
                return ChangeUserPasswordResult.OldPasswordNotValid;
            }
            user.Password = PasswordHelper.EncodePasswordMd5(changeUserPassword.Password.SanitizeText());

            await _userRepository.UpdateUser(user);
            await _userRepository.Save();

            return ChangeUserPasswordResult.Success;
        }
        #endregion

        #region User Question

        public async Task UpdateUserScoreAndMedal(long userId, int score)
        {
            var user = await GetUserById(userId);

            if (user == null) return;

            user.Score += score;

            await _userRepository.UpdateUser(user);
            await _userRepository.Save();

            if (user.Score >= _scoreManagement.MinScoreForBronzeMedal && user.Score < _scoreManagement.MinScoreForSilverMedal)
            {
                if (user.Medal != null && user.Medal == UserMedal.Bronze)
                {
                    return;
                }

                user.Medal = UserMedal.Bronze;

                await _userRepository.UpdateUser(user);
                await _userRepository.Save();
            }
            else if (user.Score >= _scoreManagement.MinScoreForSilverMedal && user.Score < _scoreManagement.MinScoreForGoldMedal)
            {
                if (user.Medal != null && user.Medal == UserMedal.Silver)
                {
                    return;
                }

                user.Medal = UserMedal.Silver;

                await _userRepository.UpdateUser(user);
                await _userRepository.Save();
            }
            else if (user.Score >= _scoreManagement.MinScoreForGoldMedal)
            {
                if (user.Medal != null && user.Medal == UserMedal.Gold)
                {
                    return;
                }

                user.Medal = UserMedal.Gold;

                await _userRepository.UpdateUser(user);
                await _userRepository.Save();
            }
        }

        #endregion

        #region Admin

        #region User
        public async Task<FilterUserAdminViewModel> FilterUserAdmin(FilterUserAdminViewModel filter)
        {
            var query = _userRepository.GetAllUsers();

            if (!string.IsNullOrEmpty(filter.UserSearch))
            {
                query = query.Where(s => (s.FirstName + " " + s.LastName).Trim().Contains(filter.UserSearch)
                || s.Email.Contains(filter.UserSearch));
            }

            switch (filter.ActivationStatus)
            {
                case AccountActivationStatus.All:
                    break;
                case AccountActivationStatus.IsActive:
                    query = query.Where(s => s.IsEmailConfirmed);
                    break;
                case AccountActivationStatus.NotActive:
                    query = query.Where(s => !s.IsEmailConfirmed);
                    break;
            }

            await filter.SetPaging(query);

            return filter;
        }

        public async Task<EditUserAdminViewModel?> FillEditUserAdminViewModel(long userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null) return null;

            return new EditUserAdminViewModel
            {
                Avatar = user.Avatar,
                BirthDate = user.BirthDate?.ToShamsi(),
                CityId = user.CityId,
                CountryId = user.CountryId,
                Description = user.Description,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                GetNewsLetter = user.GetNewsLetter,
                IsAdmin = user.IsAdmin,
                IsBan = user.IsBan,
                IsEmailConfirmed = user.IsEmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                UserId = userId,

            };

        }

        public async Task<EditUserAdminResult> EditUserAdmin(EditUserAdminViewModel editUserAdminViewModel)
        {
            var user = await _userRepository.GetUserById(editUserAdminViewModel.UserId);

            if (user == null) return EditUserAdminResult.UserNotFound;

            if (!user.Email.Equals(editUserAdminViewModel.Email) && await _userRepository.IsExistsUserByEmail(editUserAdminViewModel.Email))
            {
                return EditUserAdminResult.NotValidEmail;
            }
            user.Email = editUserAdminViewModel.Email;
            user.FirstName = editUserAdminViewModel.FirstName;
            user.LastName = editUserAdminViewModel.LastName;
            user.Avatar = editUserAdminViewModel.Avatar;
            user.Description = editUserAdminViewModel.Description;
            user.IsBan = editUserAdminViewModel.IsBan;
            user.IsEmailConfirmed = editUserAdminViewModel.IsEmailConfirmed;
            user.PhoneNumber = editUserAdminViewModel.PhoneNumber;
            user.IsAdmin = editUserAdminViewModel.IsAdmin;
            user.BirthDate = editUserAdminViewModel.BirthDate.ToMiladi();
            user.PhoneNumber = editUserAdminViewModel.PhoneNumber;
            user.CountryId = editUserAdminViewModel.CountryId;
            user.CityId = editUserAdminViewModel.CityId;

            if (!string.IsNullOrEmpty(editUserAdminViewModel.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUserAdminViewModel.Password);
            }

            await _userRepository.UpdateUser(user);
            await _userRepository.Save();

            return EditUserAdminResult.Success;
        }

        public async Task<bool> CheckUserPermission(long permissionId, long userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null) return false;

            if (user.IsAdmin) return true;

            return await _userRepository.CheckUserHasPermission(user.Id, permissionId);

        }
        #endregion

        #endregion
    }
}
