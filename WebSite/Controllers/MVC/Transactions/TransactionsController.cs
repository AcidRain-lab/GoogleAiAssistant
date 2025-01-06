using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC
{
    [Authorize(Policy = "CookiePolicy")]
    [AuthorizeRoles("Admin", "User")]
    [Controller]
    public class TransactionsController : Controller
    {
        private readonly TransactionService _transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index(Guid cardId)
        {
            if (cardId == Guid.Empty)
                return BadRequest("Invalid card ID.");

            var transactions = await _transactionService.GetTransactionsByCardIdAsync(cardId);

            return View(transactions); // Передаем список транзакций в представление
        }


        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TransactionDTO transaction)
        {
            if (ModelState.IsValid)
            {
                await _transactionService.AddTransactionAsync(transaction);
                return RedirectToAction("Index");
            }

            return View(transaction);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                TempData["Error"] = $"Transaction with ID {id} not found.";
                return RedirectToAction("Index");
            }

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TransactionDTO transaction)
        {
            if (ModelState.IsValid)
            {
                await _transactionService.UpdateTransactionAsync(transaction);
                TempData["Message"] = "Transaction updated successfully.";
                return RedirectToAction("Index");
            }

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _transactionService.DeleteTransactionAsync(id);
            TempData["Message"] = $"Transaction with ID {id} deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
