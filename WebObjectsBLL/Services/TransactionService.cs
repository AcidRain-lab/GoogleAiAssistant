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
        public async Task<List<BankAccountTransactionDTO>> GetAllTransactionsAsync()
        {
            var transactions = await _context.BankAccountTransactions
                .Include(t => t.TransactionType)
                .Include(t => t.TransactionSourceType)
                .OrderBy(t => t.TransactionDate)
                .ToListAsync();

            return _mapper.Map<List<BankAccountTransactionDTO>>(transactions);
        }

        // Добавление новой транзакции
        public async Task<BankAccountTransactionDTO> AddTransactionAsync(BankAccountTransactionDTO transactionDto)
        {
            // Маппинг DTO на сущность
            var transaction = _mapper.Map<BankAccountTransaction>(transactionDto);

            transaction.Id = Guid.NewGuid();
            transaction.TransactionDate = DateTime.Now;

            _context.BankAccountTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return _mapper.Map<BankAccountTransactionDTO>(transaction);
        }

        // Получение транзакции по ID
        public async Task<BankAccountTransactionDTO?> GetTransactionByIdAsync(Guid id)
        {
            var transaction = await _context.BankAccountTransactions
                .Include(t => t.TransactionType)
                .Include(t => t.TransactionSourceType)
                .FirstOrDefaultAsync(t => t.Id == id);

            return transaction == null ? null : _mapper.Map<BankAccountTransactionDTO>(transaction);
        }

        // Обновление транзакции
        public async Task UpdateTransactionAsync(BankAccountTransactionDTO transactionDto)
        {
            var transaction = await _context.BankAccountTransactions.FirstOrDefaultAsync(t => t.Id == transactionDto.Id);

            if (transaction != null)
            {
                _mapper.Map(transactionDto, transaction);
                await _context.SaveChangesAsync();
            }
        }

        // Удаление транзакции
        public async Task DeleteTransactionAsync(Guid id)
        {
            var transaction = await _context.BankAccountTransactions.FirstOrDefaultAsync(t => t.Id == id);

            if (transaction != null)
            {
                _context.BankAccountTransactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        // Получение транзакций по ID карты
        public async Task<IEnumerable<BankAccountTransactionDTO>> GetTransactionsByAccountIdAsync(Guid cardId)
        {
            var transactions = await _context.BankAccountTransactions
                .Where(t => t.BankCardId == cardId)
                .Include(t => t.TransactionType)
                .Include(t => t.TransactionSourceType)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BankAccountTransactionDTO>>(transactions);
        }
    }
}
