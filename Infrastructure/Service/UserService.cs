using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        private UserResponseDto MapToUserDto(User user)
        {
            return new UserResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = user.RoleId
            };
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(int skip, int take)
        {
            var users = await _repo.GetAllUsersAsync(skip, take);
            return users.Select(MapToUserDto);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _repo.GetUserByIdAsync(id);
            return user == null ? null : MapToUserDto(user);
        }

        public async Task<int> RegisterUserAsync(UserRegisterDto dto)
        {
            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var user = new User
                {
                    UserName = dto.UserName,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PasswordHash = hashedPassword
                };

                return await _repo.AddUserAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during user registration.", ex);
            }
        }

        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            try
            {
                var user = await _repo.GetUserByIdAsync(id);
                if (user == null) return false;

                MappingExtensions.PatchValues(user, dto);

                return await _repo.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user with ID {id}.", ex);
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _repo.GetUserByIdAsync(userId);
            if (user == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
                return false;

            var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            return await _repo.UpdatePasswordAsync(userId, newHash);
        }

        public async Task<bool> DeleteUserAsync(int id) => await _repo.DeleteUserAsync(id);

        public async Task<bool> ExistsUserAsync(string userName)
        {
            return await _repo.ExistsUserAsync(userName);

        }
        public async Task<UserResponseDto?> GetUserByUserNameAsync(string userName)
        {
            var user = await _repo.GetUserByUserNameAsync(userName);
            return user == null ? null : MapToUserDto(user);
        }
        public async Task<UserResponseDto?> LoginAsync(string userName, string password)
        {
            var user = await _repo.GetUserByUserNameAsync(userName);
            if (user == null) return null;
            if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
                return null;
            return MapToUserDto(user);

        }
    }
}
