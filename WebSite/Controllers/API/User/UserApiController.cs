using Microsoft.AspNetCore.Mvc;
using WebLoginBLL.Services;
using WebLoginBLL.DTO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace WebSite.Controllers.API.User
{
    [Authorize(Policy = "AdminOnly")]
    //[Authorize(Policy = "JwtPolicy")] // Указываем политику JwtPolicy
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService; // Добавляем RoleService

        public UserApiController(UserService userService, RoleService roleService)
        {
            _userService = userService;
            _roleService = roleService; // Инициализируем RoleService
        }

        [HttpGet("IndexApi")]
        public async Task<IActionResult> IndexApi()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("AddApi")]
        public async Task<IActionResult> AddApi([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid user data.");

            var (success, message) = await _userService.CreateUserAsync(userDto);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message = "User created successfully.", user = userDto });
        }

        [HttpPost("EditApi")]
        public async Task<IActionResult> EditApi([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid user data.");

            var (success, message) = await _userService.UpdateUserAsync(userDto);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message = "User updated successfully.", user = userDto });
        }

        [HttpPost("DeleteApi/{id}")]
        public async Task<IActionResult> DeleteApi(Guid id)
        {
            var (success, message) = await _userService.DeleteUserAsync(id);
            if (!success)
                return NotFound(new { message });

            return Ok(new { message });
        }

        [HttpGet("GetUserRolesApi/{id}")]
        public async Task<IActionResult> GetUserRolesApi(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            // Используем RoleService для получения ролей
            var roles = await _roleService.GetAllRolesAsync();
            var userRoles = roles.Where(r => r.Id == user.RoleId);
            return Ok(userRoles);
        }

        [HttpGet("SearchUsersApi")]
        public async Task<IActionResult> SearchUsersApi(string query)
        {
            var users = await _userService.GetAllUsersAsync();
            var filteredUsers = users.Where(u =>
                u.FirstName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                u.LastName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(query, StringComparison.OrdinalIgnoreCase));
            return Ok(filteredUsers);
        }

        [HttpPost("BulkDeleteUsersApi")]
        public async Task<IActionResult> BulkDeleteUsersApi([FromBody] List<Guid> userIds)
        {
            foreach (var id in userIds)
            {
                await _userService.DeleteUserAsync(id);
            }

            return Ok(new { message = "Users deleted successfully." });
        }
    }
}
