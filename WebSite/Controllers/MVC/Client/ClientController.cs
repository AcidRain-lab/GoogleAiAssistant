using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC
{
    public class ClientController : Controller
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllAsync();
            return View(clients);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ClientDTO clientDto)
        {
            if (!ModelState.IsValid)
            {
                return View(clientDto);
            }

            await _clientService.CreateAsync(clientDto);

            TempData["Message"] = "Client created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound();

            return View(client);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound();

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientDTO clientDto)
        {
            if (!ModelState.IsValid)
            {
                return View(clientDto);
            }

            await _clientService.UpdateAsync(clientDto);

            TempData["Message"] = "Client updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _clientService.DeleteAsync(id);
            TempData["Message"] = "Client deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
