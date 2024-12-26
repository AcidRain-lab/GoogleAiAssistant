using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers
{
    public class BankCardController : Controller
    {
        private readonly BankCardService _bankCardService;
        private readonly BankAccountService _bankAccountService;

        public BankCardController(BankCardService bankCardService, BankAccountService bankAccountService)
        {
            _bankCardService = bankCardService;
            _bankAccountService = bankAccountService;
        }

        public async Task<IActionResult> Index()
        {
            // Получение ID текущего клиента через User.Identity
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(clientId))
                return Unauthorized(); // Если клиент не залогинен, возвращаем ошибку

            var accounts = await _bankAccountService.GetByClientIdAsync(Guid.Parse(clientId));
            var cards = new List<BankCardDTO>();

            foreach (var account in accounts)
            {
                var accountCards = await _bankCardService.GetByBankAccountIdAsync(account.Id);
                cards.AddRange(accountCards);
            }

            return View(cards);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid accountId, Guid cardTypeId, string cardHolderName)
        {
            if (!ModelState.IsValid)
                return View();

            await _bankCardService.CreateAsync(accountId, cardTypeId, cardHolderName);
            return RedirectToAction(nameof(Index));
        }

    }
}
