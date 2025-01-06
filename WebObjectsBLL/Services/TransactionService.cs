using AutoMapper;
using DAL.Models;
using WebObjectsBLL.DTO;
using Microsoft.EntityFrameworkCore;

namespace WebObjectsBLL.Services
{
    public class TransactionService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public TransactionService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Получение всех транзакций
        public async Task<List<TransactionDTO>> GetAllTransactionsAsync()
        {
            var transactions = await _context.Transactions
                .OrderBy(t => t.Date)
                .ThenBy(t => t.Id)
                .ToListAsync();

            return _mapper.Map<List<TransactionDTO>>(transactions);
        }

        // Добавление новой транзакции
        public async Task<TransactionDTO> AddTransactionAsync(TransactionDTO transactionDto)
        {
            var transaction = _mapper
            .Map<Transaction>(transactionDto);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            await RecalculateBalancesAsync(); // Пересчет балансов после добавления транзакции

            return _mapper.Map<TransactionDTO>(transaction);
        }

        // Получение транзакции по ID
        public async Task<TransactionDTO?> GetTransactionByIdAsync(int id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
            return transaction == null ? null : _mapper.Map<TransactionDTO>(transaction);
        }

        // Обновление транзакции
        public async Task UpdateTransactionAsync(TransactionDTO updatedTransactionDto)
        {
            var existingTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == updatedTransactionDto.Id);
            if (existingTransaction != null)
            {
                _mapper.Map(updatedTransactionDto, existingTransaction);
                await _context.SaveChangesAsync();

                await RecalculateBalancesAsync(); // Пересчет балансов после обновления
            }
        }

        // Удаление транзакции
        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();

                await RecalculateBalancesAsync(); // Пересчет балансов после удаления
            }
        }

        // Получение транзакций по ID карты
        public async Task<IEnumerable<BankAccountTransactionDTO>> GetTransactionsByAccountIdAsync(Guid accountId)
        {
            var transactions = await _context.BankAccountTransactions
                .Where(t => t.BankAccountId == accountId)
                .Include(t => t.TransactionType) // Включение типа транзакции
                .Include(t => t.TransactionSourceType) // Включение источника транзакции
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new BankAccountTransactionDTO
                {
                    TransactionDate = t.TransactionDate,
                    TransactionType = t.TransactionType.Name,
                    Amount = t.TransactionType.Name == "Income" ? (decimal)t.Amount : -(decimal)t.Amount, // Приведение double к decimal
                    FromClientName = t.FromClientId.HasValue
                        ? _context.Clients
                            .Where(c => c.Id == t.FromClientId.Value)
                            .Select(c => c.FirstName + " " + c.LastName)
                            .FirstOrDefault() ?? "N/A"
                        : "N/A",
                    PaymentSystem = t.TransactionSourceType.Name,
                    Notes = t.Notes
                })
                .ToListAsync();

            return transactions;
        }





        // Приватный метод для пересчета балансов
        private async Task RecalculateBalancesAsync()
        {
            var transactions = await _context.Transactions
                .OrderBy(t => t.Date)
                .ThenBy(t => t.Id)
                .ToListAsync();

            double balance = 0;
            foreach (var transaction in transactions)
            {
                balance += transaction.ActionType == 1 ? transaction.Quantity : -transaction.Quantity;
                transaction.Balance = balance;
            }

            await _context.SaveChangesAsync();
        }
    }
}
