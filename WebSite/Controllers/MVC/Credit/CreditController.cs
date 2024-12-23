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
            var credit = new CreditDTO
            {
                ClientId = clientId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };
            return View(credit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreditDTO creditDto)
        {
            Console.WriteLine("Метод Add вызван."); // Логирование для проверки вызова метода

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState не валидна."); // Логирование состояния модели
                return View(creditDto);
            }

            if (creditDto.StartDate == default || creditDto.EndDate == default)
            {
                ModelState.AddModelError("", "Start Date and End Date are required.");
                return View(creditDto);
            }

            try
            {
                await _creditService.CreateAsync(creditDto);
                return RedirectToAction(nameof(Index), new { clientId = creditDto.ClientId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(creditDto);
            }
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
