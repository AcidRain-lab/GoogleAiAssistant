using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC
{
    [Authorize(Policy = "CookiePolicy")]
    [AuthorizeRoles("Admin", "User")]
    public class TransactionsController : Controller
    {
        private readonly TransactionService _transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid cardId)
        {
            if (cardId == Guid.Empty)
                return BadRequest("Invalid card ID.");

            var transactions = await _transactionService.GetTransactionsByAccountIdAsync(cardId);
            ViewBag.CardId = cardId;
            return View(transactions);
        }

        [HttpGet]
        public IActionResult Add(Guid cardId)
        {
            if (cardId == Guid.Empty)
                return BadRequest("Invalid card ID.");

            var transactionDto = new BankAccountTransactionDTO
            {
                BankCardId = cardId
            };

            return View(transactionDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(BankAccountTransactionDTO transaction)
        {
            if (transaction == null || transaction.BankCardId == Guid.Empty)
            {
                ModelState.AddModelError("", "Transaction is invalid.");
                return View(transaction);
            }

            //if (ModelState.IsValid)
            {
                await _transactionService.AddTransactionAsync(transaction);
                return RedirectToAction("Index", new { cardId = transaction.BankCardId });
            }

            return View(transaction);
        }

    }
}
