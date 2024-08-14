using Imigration.Application.Generators;
using Imigration.Application.Security;
using Imigration.Application.Services.Interfaces;
using Imigration.Application.Statics;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Interfaces;
using Imigration.Domains.ViewModels.Account;

namespace Imigration.Application.Services.Implementions
{
    public class UserService : IUserService
    {
        #region Ctor
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region Register

        public async Task<RegisterResult> RegisterUser(RegisterViewModel register)
        {
            // Check Email Exists
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
            await _userRepository.CreateUser(user);
            await _userRepository.Save();

            #region Send Activation Email

            var body = $@"
                <div> برای فعالسازی حساب کاربری خود روی لینک زیر کلیک کنید . </div>
                <a href='{PathTools.SiteAddress}/Activate-Email/{user.EmailActivationCode}'>فعالسازی حساب کاربری</a>
                ";

            await _emailService.SendEmail(user.Email, "فعالسازی حساب کاربری", body);

            #endregion

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
    }
}
