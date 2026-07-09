using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
   public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(int skip, int take);
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<UserResponseDto?> GetUserByUserNameAsync(string userName);

        Task<int> RegisterUserAsync(UserRegisterDto dto);

        Task<bool> UpdateUserAsync(int id, UserUpdateDto dto);

        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<UserResponseDto?> LoginAsync(string userName, string password);

        Task<bool> DeleteUserAsync(int id);

        Task<bool> ExistsUserAsync(string userName);
    }
}
