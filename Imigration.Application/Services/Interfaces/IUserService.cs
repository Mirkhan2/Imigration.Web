using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.ViewModels.Account;
using Imigration.Domains.ViewModels.UserPanel.Account;

namespace Imigration.Application.Services.Interfaces
{
    public interface IUserService
    {
        #region Register

        Task<RegisterResult> RegisterUser(RegisterViewModel register);

        #endregion

        #region Login
        Task<LoginResult> CheckUserForLogin(LoginViewModel login);
        Task<User> GetUserByEmail(string email);
        #endregion

        #region Email Activation
        Task<bool> ActivateUserEmail(string activationCode);
        #endregion

        #region Forgot Password

        Task<ForgotPasswordResult> ForgotPassword(ForgotPasswordViewModel forgotPassword);
        #endregion

        #region RAeset Password

        Task<ResetPasswordResult>  ResetPassword(ResetPasswordViewModel resetPassword);
        Task<User> GetUserByActivationCode(string activationCode);
        #endregion

        #region USer Panel
        Task<User?> GetUserById(long userId);
        Task ChangeUserAvatar(long userId, string fileName);
        Task<EditUserViewModel> FillEditUserViewModel(long userId);
        Task<EditUserInfoResult> EditUserInfo(EditUserViewModel editUserViewModel, long userId);
        Task<ChangeUserPasswordResult> ChangeUserPassword(long userId, ChangeUserPasswordViewModel changeUserPasswor);
     
        #endregion
    }
}
