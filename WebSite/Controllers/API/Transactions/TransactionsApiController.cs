/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.API.Transactions
{
    [Authorize(Policy = "JwtPolicy")] // Указываем политику JwtPolicy
    [AuthorizeRoles("Admin", "User")]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsApiController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionsApiController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionsAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error.", details = ex.Message });
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    return NotFound(new { message = $"Transaction with Id {id} not found." });
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error.", details = ex.Message });
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionDTO transaction)
        {
            try
            {
                if (transaction == null || transaction.Quantity <= 0 || transaction.Date == default)
                {
                    return BadRequest(new { message = "Invalid transaction data." });
                }

                var addedTransaction = await _transactionService.AddTransactionAsync(transaction);
                return Ok(new { message = "Transaction added successfully.", transaction = addedTransaction });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error.", details = ex.Message });
            }
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionDTO transaction)
        {
            try
            {
                if (transaction == null || transaction.Quantity <= 0 || transaction.Date == default)
                {
                    return BadRequest(new { message = "Invalid transaction data." });
                }

                transaction.Id = id;
                await _transactionService.UpdateTransactionAsync(transaction);
                return Ok(new { message = "Transaction updated successfully.", transaction });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error.", details = ex.Message });
            }
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    return NotFound(new { message = $"Transaction with Id {id} not found." });
                }

                await _transactionService.DeleteTransactionAsync(id);
                return Ok(new { message = $"Transaction with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error.", details = ex.Message });
            }
        }
    }
}
*/