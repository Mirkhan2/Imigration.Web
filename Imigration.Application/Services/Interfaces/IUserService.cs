using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.ViewModels.Account;

namespace Imigration.Application.Services.Interfaces
{
    public interface IUserService
    {
        #region Register

        Task<RegisterResult> RegisterUser(RegisterViewModel register);
      
        #endregion
    }
}
