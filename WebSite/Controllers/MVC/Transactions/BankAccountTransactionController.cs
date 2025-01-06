using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.Services;
using WebObjectsBLL.DTO;

public class BankAccountTransactionController : Controller
{
    private readonly TransactionService _transactionService;

    public BankAccountTransactionController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<IActionResult> Index(Guid cardId)
    {
        if (cardId == Guid.Empty)
            return BadRequest("Invalid card ID.");

        // Получение транзакций, привязанных к карте
        var transactions = await _transactionService.GetTransactionsByCardIdAsync(cardId);

        return View(transactions);
    }
}
