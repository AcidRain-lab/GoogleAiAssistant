using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC
{
    public class ClientProfileController : Controller
    {
        private readonly ClientProfileService _clientProfileService;

        public ClientProfileController(ClientProfileService clientProfileService)
        {
            _clientProfileService = clientProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var profile = await _clientProfileService.GetClientProfileAsync(id);
            if (profile == null)
            {
                TempData["Error"] = "Client profile not found.";
                return RedirectToAction("Index", "Client");
            }

            return View(profile);
        }
    }
}
