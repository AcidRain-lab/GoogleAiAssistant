using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC
{
    [Authorize]
    public class CreditController : Controller
    {
        private readonly CreditService _creditService;

        public CreditController(CreditService creditService)
        {
            _creditService = creditService;
        }

        public async Task<IActionResult> Index(Guid clientId)
        {
            var credits = await _creditService.GetByClientIdAsync(clientId);
            ViewBag.ClientId = clientId;
            return View(credits);
        }

        public IActionResult Add(Guid clientId)
        {
            var credit = new CreditDTO { ClientId = clientId };
            return View(credit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreditDTO creditDto)
        {
            if (!ModelState.IsValid)
            {
                return View(creditDto);
            }

            var clientExists = await _creditService.ClientExistsAsync(creditDto.ClientId);
            if (!clientExists)
            {
                ModelState.AddModelError("", "Specified client does not exist.");
                return View(creditDto);
            }

            await _creditService.CreateAsync(creditDto);
            return RedirectToAction(nameof(Index), new { clientId = creditDto.ClientId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var credit = await _creditService.GetByIdAsync(id);
            if (credit == null)
                return NotFound();

            return View(credit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreditDTO creditDto)
        {
            if (!ModelState.IsValid)
            {
                return View(creditDto);
            }

            await _creditService.UpdateAsync(creditDto);
            return RedirectToAction(nameof(Index), new { clientId = creditDto.ClientId });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var credit = await _creditService.GetByIdAsync(id);
            if (credit == null)
                return NotFound();

            return View(credit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid clientId)
        {
            await _creditService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { clientId });
        }
    }
}
