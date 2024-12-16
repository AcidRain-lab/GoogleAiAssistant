using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAuthCoreBLL.SecureByRoleClasses;
using WebLoginBLL.DTO;
using WebLoginBLL.Services;


namespace WebSite.Controllers.MVC.User
{
    [Authorize(Policy = "CookiePolicy")]
    [AuthorizeRoles("Admin")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public UserController(UserService userService, RoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.Roles = await _roleService.GetAllRolesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserDTO user)
        {
            if (ModelState.IsValid)
            {
                var (success, message) = await _userService.CreateUserAsync(user);
                if (success)
                {
                    TempData["Message"] = message;
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, message);
            }

            ViewBag.Roles = await _roleService.GetAllRolesAsync(); // Вернуть список ролей при ошибке
            return View(user);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = $"User with ID {id} not found.";
                return RedirectToAction("Index");
            }

            ViewBag.Roles = await _roleService.GetAllRolesAsync();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserDTO user)
        {
            if (ModelState.IsValid)
            {
                var (success, message) = await _userService.UpdateUserAsync(user);
                if (success)
                {
                    TempData["Message"] = message;
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, message);
            }

            ViewBag.Roles = await _roleService.GetAllRolesAsync();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var (success, message) = await _userService.DeleteUserAsync(id);
            TempData["Message"] = message;
            return RedirectToAction("Index");
        }
    }
}
