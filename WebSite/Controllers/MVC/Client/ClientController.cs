using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        // Отображение всех клиентов
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllAsync();
            return View(clients);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var client = await _clientService.GetDetailByIdAsync(id);
                if (client == null)
                {
                    Console.WriteLine($"Client with ID {id} not found.");
                    return NotFound();
                }

                return View(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }


        #region Добавление клиента
        // Страница добавления клиента
        public IActionResult Add()
        {
            var clientDto = new ClientDetailDTO();
            return View(clientDto);
        }

        // Обработка добавления клиента
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ClientDetailDTO clientDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return View(clientDetailDto);
            }

            try
            {
                await _clientService.CreateAsync(clientDetailDto);
                TempData["Message"] = "Client created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                return View(clientDetailDto);
            }
        }
        #endregion

        #region Редактирование клиента
        // Страница редактирования клиента
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = await _clientService.GetDetailByIdAsync(id);
            if (client == null)
                return NotFound();

            return View(client);
        }

        // Обработка редактирования клиента
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientDetailDTO clientDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return View(clientDetailDto);
            }

            try
            {
                await _clientService.UpdateAsync(clientDetailDto);
                TempData["Message"] = "Client updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(clientDetailDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                return View(clientDetailDto);
            }
        }
        #endregion

        #region Удаление клиента
        // Удаление клиента
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _clientService.DeleteAsync(id);
                TempData["Message"] = "Client deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion
    }
}
