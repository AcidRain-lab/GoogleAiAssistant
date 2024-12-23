using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLoginBLL.DTO;
using WebLoginBLL.Services;
using WebAuthCoreBLL.SecureByRoleClasses;

namespace WebSite.Controllers.MVC.User
{
    [Authorize(Policy = "CookiePolicy")]
    [AuthorizeRoles("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return View(roles);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleDTO role)
        {
            if (ModelState.IsValid)
            {
                await _roleService.CreateRoleAsync(role);
                TempData["Message"] = "Role created successfully.";
                return RedirectToAction("Index");
            }
            return View(role);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                TempData["Error"] = $"Role with ID {id} not found.";
                return RedirectToAction("Index");
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleDTO role)
        {
            if (ModelState.IsValid)
            {
                await _roleService.UpdateRoleAsync(role);
                TempData["Message"] = "Role updated successfully.";
                return RedirectToAction("Index");
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                TempData["Error"] = $"Role with ID {id} not found.";
                return RedirectToAction("Index");
            }

            await _roleService.DeleteRoleAsync(id);
            TempData["Message"] = $"Role with ID {id} deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
