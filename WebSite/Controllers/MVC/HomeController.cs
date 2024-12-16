using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;
using WebSite.Models;
using WebAuthCoreBLL.SecureByRoleClasses;

namespace WebSite.Controllers.MVC
{
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]

    //[LayoutByRole]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["IsAuthenticated"] = User.Identity?.IsAuthenticated ?? false;
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            ViewData["UserRole"] = HttpContext.Session.GetString("UserRole");
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
