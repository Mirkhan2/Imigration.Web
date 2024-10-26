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
            //return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<User> GetUserByActivationCode(string activationCode)
        {
            return await _context.Users.FirstOrDefaultAsync(s => s.EmailActivationCode.Equals(activationCode));
        }
        public async Task<User?> GetUserById(long userId)
        {
            return await _context.Users.FirstOrDefaultAsync(s => !s.IsDelete && s.Id == userId);
        }
        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }


        public async Task UpdateUser(User user)
        {
           _context.Update(user);
        }
        public IQueryable<User> GetAllUsers()
        {
            return _context.Users.Where(s => !s.IsDelete).AsQueryable();
        }

        public async Task<bool> CheckUserHasPermission(long userId, long permissionId)
        {
            return await _context.UserPermissions
                .AnyAsync(s => s.UserId == userId && s.PermissionId == permissionId);
        }
    }
}
