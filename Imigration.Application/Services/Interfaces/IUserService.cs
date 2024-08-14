using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.ViewModels.Account;

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
    }
}
