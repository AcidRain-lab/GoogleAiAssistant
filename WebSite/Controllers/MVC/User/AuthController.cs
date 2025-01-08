using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebLoginBLL.DTO;
using WebLoginBLL.Services;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC.User
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;
        private readonly ClientService _clientService; // Для работы с клиентами

        public AuthController(AuthService authService, UserService userService, ClientService clientService)
        {
            _authService = authService;
            _userService = userService;
            _clientService = clientService;
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

            // Устанавливаем аутентификацию через сервис
            await _authService.SetCookieAuthentication(user);

            // Получаем клиента, связанного с пользователем
            var client = await _clientService.GetClientByUserIdAsync(user.Id);

            // Сохраняем ID клиента в сессию
            if (client != null)
            {
                HttpContext.Session.SetString("ActiveClientId", client.Id.ToString());
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Очищаем сессию и куки через сервис
            await _authService.Logout();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
