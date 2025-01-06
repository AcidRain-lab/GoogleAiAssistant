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

        public async Task<List<BankAccountTransactionDTO>> GetAllTransactionsAsync()
        {
            var transactions = await _context.BankAccountTransactions
                .Include(t => t.TransactionType)
                .Include(t => t.TransactionSourceType)
                .OrderBy(t => t.TransactionDate)
                .ToListAsync();

            return _mapper.Map<List<BankAccountTransactionDTO>>(transactions);
        }

        /*public async Task<BankAccountTransactionDTO> AddTransactionAsync(BankAccountTransactionDTO transactionDto)
        {
            var transaction = _mapper.Map<BankAccountTransaction>(transactionDto);
            transaction.Id = Guid.NewGuid();
            transaction.TransactionDate = DateTime.Now;

            _context.BankAccountTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return _mapper.Map<BankAccountTransactionDTO>(transaction);
        }*/
        public async Task<BankAccountTransactionDTO> AddTransactionAsync(BankAccountTransactionDTO transactionDto)
        {
            // Устанавливаем дефолтные значения
            string defaultTransactionType = "Expenses";
            string defaultPaymentSystem = "Visa";

            // Проверяем, передано ли значение TransactionType
            string transactionTypeName = string.IsNullOrWhiteSpace(transactionDto.TransactionType)
                ? defaultTransactionType
                : transactionDto.TransactionType;

            // Получаем TransactionType из базы данных
            var transactionType = await _context.TransactionTypes
                .FirstOrDefaultAsync(tt => tt.Name == transactionTypeName);

            if (transactionType == null)
            {
                throw new KeyNotFoundException($"TransactionType '{transactionTypeName}' not found.");
            }

            // Проверяем PaymentSystem
            string paymentSystemName = string.IsNullOrWhiteSpace(transactionDto.PaymentSystem)
                ? defaultPaymentSystem
                : transactionDto.PaymentSystem;

            var transactionSourceType = await _context.TransactionSourceTypes
                .FirstOrDefaultAsync(ts => ts.Name == paymentSystemName);

            if (transactionSourceType == null)
            {
                throw new KeyNotFoundException($"TransactionSourceType '{paymentSystemName}' not found.");
            }

            // Маппинг DTO на сущность
            var transaction = _mapper.Map<BankAccountTransaction>(transactionDto);
            transaction.Id = Guid.NewGuid();
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = transactionType;
            transaction.TransactionSourceType = transactionSourceType;

            // Добавляем транзакцию в БД
            _context.BankAccountTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return _mapper.Map<BankAccountTransactionDTO>(transaction);
        }





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
