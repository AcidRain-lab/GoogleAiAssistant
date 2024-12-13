using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using WebLoginBLL.DTO;
using WebLoginBLL.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebSite.Controllers.MVC.User
{
    [AllowAnonymous]
    [Controller]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;

        public AuthController(AuthService authService, UserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult LoginBasic()
        {
            return View(new LoginDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginBasic(LoginDTO model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userService.ValidateUserAsync(model.UserName, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            // Используем метод сервиса для установки куков и записи в сессию
            await _authService.SetCookieAuthentication(user);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
       

            // Выходим из системы
            await _authService.Logout();

            // Перенаправляем на главную страницу
            return RedirectToAction("Index", "Home");
        }
    }
}
