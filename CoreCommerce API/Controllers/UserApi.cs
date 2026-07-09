using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce_API.Controllers
{
    [Route("api/UserApi")]
    [ApiController]
    public class UserApi : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _repo; // أو اسم الـ Repository الخاص بك
        public UserApi(IUserService userService,IUserRepository repo)
        {
            _userService = userService;
            _repo = repo;
        }
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login(UserLoginDto dto)
        {
            var user = await _userService.LoginAsync(dto.UserName, dto.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password." });
            return Ok(user);
        }
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (await _userService.ExistsUserAsync(dto.UserName))
                return BadRequest(new { message = "Username is already taken." });
            var userid = await _userService.RegisterUserAsync(dto);
            return Ok(new { message = "User registered successfully.", userid });
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { message = "User not found." });

            return Ok(user);
        }
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var users = await _userService.GetAllUsersAsync(skip, take);
            return Ok(users);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, [FromBody] UserUpdateDto dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);
            if (!result) return NotFound(new { message = "User not found or update failed." });

            return Ok(new { message = "User updated successfully." });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound(new { message = "User not found." });

            return Ok(new { message = "User deleted successfully." });
        }
        [HttpPatch("{id}/password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto dto)
        {
            var result = await _userService.ChangePasswordAsync(id, dto.OldPassword, dto.NewPassword);

            if (!result)
                return BadRequest(new { message = "Failed to change password. Check your old password or user ID." });

            return Ok(new { message = "Password updated successfully." });
        }
        [HttpGet("fix-passwords-emergency")]
        public async Task<IActionResult> FixPasswordsEmergency()
        {
            // جلب كل المستخدمين مباشرة من قاعدة البيانات
            var users = await _repo.GetAllUsersAsync(0,1000);

            foreach (var user in users)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234");
               await _repo.UpdateUserAsync(user); // تحديث الـ Entity مباشرة
            }

          
            return Ok("تم التحديث.");
        }

    }
}
