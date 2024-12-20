using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC
{
    [Authorize]
    public class DepositController : Controller
    {
        private readonly DepositService _depositService;

        public DepositController(DepositService depositService)
        {
            _depositService = depositService;
        }

        public async Task<IActionResult> Index(Guid clientId)
        {
            var deposits = await _depositService.GetByClientIdAsync(clientId);
            ViewBag.ClientId = clientId;
            return View(deposits);
        }

        public IActionResult Add(Guid clientId)
        {
            var deposit = new DepositDTO { ClientId = clientId };
            return View(deposit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DepositDTO depositDto)
        {
            if (!ModelState.IsValid)
            {
                return View(depositDto);
            }

            var clientExists = await _depositService.ClientExistsAsync(depositDto.ClientId);
            if (!clientExists)
            {
                ModelState.AddModelError("", "Specified client does not exist.");
                return View(depositDto);
            }

            await _depositService.CreateAsync(depositDto);
            return RedirectToAction(nameof(Index), new { clientId = depositDto.ClientId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var deposit = await _depositService.GetByIdAsync(id);
            if (deposit == null)
                return NotFound();

            return View(deposit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepositDTO depositDto)
        {
            if (!ModelState.IsValid)
            {
                return View(depositDto);
            }

            await _depositService.UpdateAsync(depositDto);
            return RedirectToAction(nameof(Index), new { clientId = depositDto.ClientId });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var deposit = await _depositService.GetByIdAsync(id);
            if (deposit == null)
                return NotFound();

            return View(deposit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid clientId)
        {
            await _depositService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { clientId });
        }
    }
}
