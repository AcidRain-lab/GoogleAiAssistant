using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC.BankCard
{
    [Authorize]
    public class CardTypesController : Controller
    {
        private readonly BankCardService _bankCardService;

        public CardTypesController(BankCardService bankCardService)
        {
            _bankCardService = bankCardService;
        }

        public async Task<IActionResult> Index(Guid bankAccountId)
        {
            var cards = await _bankCardService.GetByBankAccountIdAsync(bankAccountId);
            ViewBag.BankAccountId = bankAccountId;
            return View(cards);
        }

        public IActionResult Add(Guid bankAccountId)
        {
            var card = new BankCardDTO
            {
                BankAccountId = bankAccountId,
                ExpirationDate = DateTime.Now.AddYears(3) // Устанавливаем дату по умолчанию
            };
            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(BankCardDTO cardDto)
        {
            if (!ModelState.IsValid)
                return View(cardDto);

            await _bankCardService.CreateAsync(cardDto);
            return RedirectToAction(nameof(Index), new { bankAccountId = cardDto.BankAccountId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var card = await _bankCardService.GetByIdAsync(id);
            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BankCardDTO cardDto)
        {
            if (!ModelState.IsValid)
                return View(cardDto);

            await _bankCardService.UpdateAsync(cardDto);
            return RedirectToAction(nameof(Index), new { bankAccountId = cardDto.BankAccountId });
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var card = await _bankCardService.GetByIdAsync(id);
            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid bankAccountId)
        {
            await _bankCardService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { bankAccountId });
        }

    }

}
