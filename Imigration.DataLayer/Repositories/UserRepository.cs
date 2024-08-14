﻿using Imigration.DataLayer.Context;
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

        #endregion

        public async Task<bool> IsExistsUserByEmail(string email)
        {
            return await _context.Users.AnyAsync(s => s.Email == email);

        }

        public async Task CreateUser(User user)
        {
           await _context.Users.AddAsync(user); 
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(s => s.Email.Equals(email));
        }

        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByActivationCode(string activationCode)
        {
            return await _context.Users.FirstOrDefaultAsync(s=> s.EmailActivationCode.Equals(activationCode));  
        }

        public async Task UpdateUser(User user)
        {
           _context.Update(user);
        }
    }
}
