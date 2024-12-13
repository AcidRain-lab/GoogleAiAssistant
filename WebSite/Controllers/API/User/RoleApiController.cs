using Microsoft.AspNetCore.Mvc;
using WebLoginBLL.Services;
using WebLoginBLL.DTO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebSite.Controllers.API.User
{
    [Authorize(Policy = "JwtPolicy")] // Указываем политику JwtPolicy
    [Route("api/[controller]")]
    [ApiController]
    public class RoleApiController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleApiController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("IndexApi")]
        public async Task<IActionResult> IndexApi()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpPost("AddApi")]
        public async Task<IActionResult> AddApi([FromBody] RoleDTO roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid role data.");

            await _roleService.CreateRoleAsync(roleDto);
            return Ok(new { message = "Role created successfully.", role = roleDto });
        }

        [HttpPost("EditApi")]
        public async Task<IActionResult> EditApi([FromBody] RoleDTO roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid role data.");

            await _roleService.UpdateRoleAsync(roleDto);
            return Ok(new { message = "Role updated successfully.", role = roleDto });
        }

        [HttpPost("DeleteApi/{id}")]
        public async Task<IActionResult> DeleteApi(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound(new { message = "Role not found." });

            await _roleService.DeleteRoleAsync(id);
            return Ok(new { message = "Role deleted successfully." });
        }

        [HttpGet("SearchRolesApi")]
        public async Task<IActionResult> SearchRolesApi(string query)
        {
            var roles = await _roleService.GetAllRolesAsync();
            var filteredRoles = roles.Where(r =>
                r.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
            return Ok(filteredRoles);
        }

        [HttpGet("GetUsersInRoleApi/{roleId}")]
        public async Task<IActionResult> GetUsersInRoleApi(int roleId)
        {
            var users = await _roleService.GetUsersInRoleAsync(roleId);
            return Ok(users);
        }

        [HttpPost("BulkDeleteRolesApi")]
        public async Task<IActionResult> BulkDeleteRolesApi([FromBody] List<int> roleIds)
        {
            foreach (var id in roleIds)
            {
                await _roleService.DeleteRoleAsync(id);
            }

            return Ok(new { message = "Roles deleted successfully." });
        }
    }
}
