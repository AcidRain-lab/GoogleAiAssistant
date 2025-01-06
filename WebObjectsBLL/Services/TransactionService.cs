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

        public async Task<List<TransactionDTO>> GetAllTransactionsAsync()
        {
            var transactions = await _context.Transactions
                .OrderBy(t => t.Date)
                .ThenBy(t => t.Id)
                .ToListAsync();
            return _mapper.Map<List<TransactionDTO>>(transactions);
        }

        public async Task<TransactionDTO> AddTransactionAsync(TransactionDTO transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            await RecalculateBalancesAsync();

            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO?> GetTransactionByIdAsync(int id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
            return transaction == null ? null : _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task UpdateTransactionAsync(TransactionDTO updatedTransactionDto)
        {
            var existingTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == updatedTransactionDto.Id);
            if (existingTransaction != null)
            {
                _mapper.Map(updatedTransactionDto, existingTransaction);
                await _context.SaveChangesAsync();

                await RecalculateBalancesAsync();
            }
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();

                await RecalculateBalancesAsync();
            }
        }

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
        public async Task<IEnumerable<TransactionDTO>> GetByCardIdAsync(Guid cardId)
        {
            var transactions = await _context.BankAccountTransactions
                .Where(t => t.BankCardId == cardId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        }
        public async Task<IEnumerable<BankAccountTransactionDTO>> GetTransactionsByCardIdAsync(Guid cardId)
        {
            var transactions = await _context.BankAccountTransactions
                .Where(t => t.BankCardId == cardId) // Фильтрация по ID карты
                .Include(t => t.TransactionType) // Подключение типа транзакции
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BankAccountTransactionDTO>>(transactions);
        }




    }
}
