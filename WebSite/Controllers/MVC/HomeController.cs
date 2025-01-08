using MediaLib.DTO;
using MediaLib.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebSite.Models;
using WebAuthCoreBLL.SecureByRoleClasses;
using WebObjectsBLL.Services;
using System.Security.Claims;

namespace WebSite.Controllers.MVC
{
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[LayoutByRole]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AvatarService _avatarService;
        private readonly ClientService _clientService; // Добавлено

        public HomeController(ILogger<HomeController> logger, AvatarService avatarService, ClientService clientService)
        {
            _logger = logger;
            _avatarService = avatarService;
            _clientService = clientService; // Инициализация
        }

        public async Task<IActionResult> Index()
        {
            ViewData["IsAuthenticated"] = User.Identity?.IsAuthenticated ?? false;
            ViewData["UserName"] = HttpContext.Session.GetString("ActiveLoginedUser");
            ViewData["UserRole"] = HttpContext.Session.GetString("UserRole");
            ViewBag.UserAvatar = "/images/default-avatar.png"; // Дефолтный аватар

            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim != null)
                {
                    try
                    {
                        Guid userId = Guid.Parse(userIdClaim);

                        // Загружаем аватар пользователя через сервис
                        var avatar = await _avatarService.GetAvatarAsync(userId);

                        ViewBag.UserAvatar = avatar?.Base64Image != null
                            ? $"data:image/{avatar.Extension};base64,{avatar.Base64Image}"
                            : "/images/default-avatar.png";
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to load user avatar.");
                    }
                }
            }

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
