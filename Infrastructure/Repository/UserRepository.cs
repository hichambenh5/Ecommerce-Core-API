using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly EcommerceDbContext _context;
        public UserRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllUsersAsync(int skip, int take)
        {
            return await _context.Users.Skip(skip).Take(take).AsNoTracking().ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);
        }
        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == userName);
        }
        public async Task<int> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.UserId;
        }
        public async Task<bool> UpdateUserAsync(User updateuser)
        {
            var user = await _context.Users.FindAsync(updateuser.UserId);
            if (user == null) return false;
            MappingExtensions.PatchValues(user, updateuser);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdatePasswordAsync(int userId, string newHash)
        {
            int affectedRows = await _context.Users.Where(u => u.UserId == userId).ExecuteUpdateAsync(s => s.SetProperty(u => u.PasswordHash, newHash));
            return affectedRows >0;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
             _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> ExistsUserAsync(string userName)
        {
            return await _context.Users.AnyAsync(u => u.UserName == userName);
        }
    }
}
