using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC.Transactions
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
        public IActionResult AddCardTransfer(Guid cardId)
        {
            if (cardId == Guid.Empty)
                return BadRequest("Invalid card ID.");

            var transactionDto = new BankAccountTransactionDTO
            {
                BankCardId = cardId
            };

            return View("AddCardTransfer", transactionDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCardTransfer(BankAccountTransactionDTO transaction)
        {
            if (transaction == null || transaction.BankCardId == Guid.Empty)
            {
                ModelState.AddModelError("", "Transaction is invalid.");
                return View("AddCardTransfer", transaction);
            }

            await _transactionService.AddCardTransferAsync(transaction);
            return RedirectToAction("Index", new { cardId = transaction.BankCardId });
        }

        [HttpGet]
        public IActionResult AddIbanTransfer(Guid cardId)
        {
            if (cardId == Guid.Empty)
                return BadRequest("Invalid card ID.");

            var transactionDto = new BankAccountTransactionDTO
            {
                BankCardId = cardId
            };

            return View("AddIbanTransfer", transactionDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddIbanTransfer(BankAccountTransactionDTO transaction)
        {
            if (transaction == null || transaction.BankCardId == Guid.Empty || string.IsNullOrWhiteSpace(transaction.Iban))
            {
                ModelState.AddModelError("", "Transaction is invalid.");
                return View("AddIbanTransfer", transaction);
            }

            await _transactionService.AddIbanTransferAsync(transaction);
            return RedirectToAction("Index", new { cardId = transaction.BankCardId });
        }

        [HttpGet]
        public IActionResult AddRequisitesTransfer(Guid cardId)
        {
            if (cardId == Guid.Empty)
                return BadRequest("Invalid card ID.");

            var transactionDto = new BankAccountTransactionDTO
            {
                BankCardId = cardId
            };

            return View("AddRequisitesTransfer", transactionDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRequisitesTransfer(BankAccountTransactionDTO transaction)
        {
            if (transaction == null || transaction.BankCardId == Guid.Empty || string.IsNullOrWhiteSpace(transaction.Mfo))
            {
                ModelState.AddModelError("", "Transaction is invalid.");
                return View("AddRequisitesTransfer", transaction);
            }

            await _transactionService.AddRequisitesTransferAsync(transaction);
            return RedirectToAction("Index", new { cardId = transaction.BankCardId });
        }
    }
}
