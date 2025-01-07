using MediaLib.DTO;
using MediaLib.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebSite.Models;
using WebAuthCoreBLL.SecureByRoleClasses;
using WebObjectsBLL.Services;

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
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            ViewData["UserRole"] = HttpContext.Session.GetString("UserRole");

            if (User.Identity?.IsAuthenticated == true)
            {
                try
                {
                    // Получаем GUID текущего пользователя из claims
                    var userIdClaim = User.FindFirst("UserId")?.Value;

                    if (userIdClaim == null)
                    {
                        throw new Exception("UserId claim is missing.");
                    }

                    Guid userId = Guid.Parse(userIdClaim);

                    // Проверяем, сохранен ли ActiveClientId в сессии
                    string? activeClientId = HttpContext.Session.GetString("ActiveClientId");

                    if (string.IsNullOrEmpty(activeClientId))
                    {
                        // Если ActiveClientId нет в сессии, получаем клиента, связанного с userId
                        var client = await _clientService.GetClientByUserIdAsync(userId);

                        if (client != null)
                        {
                            // Сохраняем ActiveClientId в сессию
                            HttpContext.Session.SetString("ActiveClientId", client.Id.ToString());
                        }
                        else
                        {
                            _logger.LogWarning($"No client found for UserId: {userId}");
                        }
                    }

                    // Загружаем аватар пользователя
                    var avatar = await _avatarService.GetAvatarAsync(userId);

                    // Устанавливаем Base64 строку для отображения аватара
                    ViewBag.UserAvatar = avatar?.Base64Image != null
                        ? $"data:image/{avatar.Extension};base64,{avatar.Base64Image}"
                        : "/images/default-avatar.png";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to load user avatar or active client.");
                    ViewBag.UserAvatar = "/images/default-avatar.png"; // Устанавливаем аватар по умолчанию в случае ошибки
                }
            }
            else
            {
                // Устанавливаем дефолтную картинку для неавторизованных пользователей
                ViewBag.UserAvatar = "/images/default-avatar.png";
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
