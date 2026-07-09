using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync(int skip, int take);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUserNameAsync(string userName);
        Task<int> AddUserAsync(User user);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> UpdatePasswordAsync(int userId, string newHash);

        Task<bool> DeleteUserAsync(int id);
        Task<bool> ExistsUserAsync(string userName);
    }
}
