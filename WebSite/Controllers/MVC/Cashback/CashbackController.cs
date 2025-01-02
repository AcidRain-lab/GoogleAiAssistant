using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC
{
    [Authorize]
    public class CashbackController : Controller
    {
        private readonly CashbackService _cashbackService;

        public CashbackController(CashbackService cashbackService)
        {
            _cashbackService = cashbackService;
        }

        public async Task<IActionResult> Index(Guid clientId)
        {
            var cashbacks = await _cashbackService.GetByClientIdAsync(clientId);
            ViewBag.ClientId = clientId;
            return PartialView("Index", cashbacks); // Используем PartialView
        }


        public IActionResult Add(Guid clientId)
        {
            var cashback = new CashbackDTO { ClientId = clientId };
            return View(cashback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CashbackDTO cashbackDto)
        {
            if (!ModelState.IsValid)
            {
                return View(cashbackDto);
            }

            await _cashbackService.CreateAsync(cashbackDto);
            return RedirectToAction(nameof(Index), new { clientId = cashbackDto.ClientId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var cashback = await _cashbackService.GetByIdAsync(id);
            if (cashback == null)
                return NotFound();

            return View(cashback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CashbackDTO cashbackDto)
        {
            if (!ModelState.IsValid)
            {
                return View(cashbackDto);
            }

            await _cashbackService.UpdateAsync(cashbackDto);
            return RedirectToAction(nameof(Index), new { clientId = cashbackDto.ClientId });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var cashback = await _cashbackService.GetByIdAsync(id);
            if (cashback == null)
                return NotFound();

            return View(cashback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid clientId)
        {
            await _cashbackService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { clientId });
        }
    }
}
