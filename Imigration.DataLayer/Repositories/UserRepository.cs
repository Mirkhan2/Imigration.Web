using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.DataLayer.Context;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Imigration.DataLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Ctor

        private readonly ImigrationDbContext _context;
        public UserRepository(ImigrationDbContext context)
        {
            _context = context;
        }

        public Task CreateUser(User user)
        {
            throw new NotImplementedException();
        }


        #endregion
        public async Task<bool> IsExistsUserByEmail(string email)
        {
            return await _context.Users.AnyAsync(s => s.Email == email);

        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
