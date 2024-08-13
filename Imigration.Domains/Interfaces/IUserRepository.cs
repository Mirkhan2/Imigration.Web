using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Account;

namespace Imigration.Domains.Interfaces
{
    public interface IUserRepository
    {

        Task<bool> IsExistsUserByEmail(string email);

        Task CreateUser(User user);

        Task Save();
    }
}
