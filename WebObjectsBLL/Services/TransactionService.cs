using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebObjectsBLL.DTO;

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
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return _mapper.Map<List<BankAccountTransactionDTO>>(transactions);
        }

        public async Task<IEnumerable<BankAccountTransactionDTO>> GetTransactionsByAccountIdAsync(Guid accountId)
        {
            var transactions = await _context.BankAccountTransactions
                .Where(t => t.BankAccountId == accountId)
                .Include(t => t.TransactionType)
                .Include(t => t.TransactionSourceType)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BankAccountTransactionDTO>>(transactions);
        }

        public async Task<IEnumerable<BankAccountTransactionDTO>> GetTransactionsByCardIdAsync(Guid cardId)
        {
            var transactions = await _context.BankAccountTransactions
                .Where(t => t.BankCardId == cardId)
                .Include(t => t.TransactionType)
                .Include(t => t.TransactionSourceType)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BankAccountTransactionDTO>>(transactions);
        }

        public async Task AddCardTransferAsync(BankAccountTransactionDTO transactionDto)
        {
            if (transactionDto.BankCardId == Guid.Empty)
            {
                throw new ArgumentException("Bank card ID is invalid.");
            }

            var transactionType = await _context.TransactionTypes
                .FirstOrDefaultAsync(tt => tt.Name == transactionDto.TransactionType);

            if (transactionType == null)
            {
                throw new KeyNotFoundException($"Transaction type '{transactionDto.TransactionType}' not found.");
            }

            var transaction = _mapper.Map<BankAccountTransaction>(transactionDto);
            transaction.Id = Guid.NewGuid();
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = transactionType;

            _context.BankAccountTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task AddIbanTransferAsync(BankAccountTransactionDTO transactionDto)
        {
            if (string.IsNullOrWhiteSpace(transactionDto.Iban))
            {
                throw new ArgumentException("IBAN is required for this transfer.");
            }

            var transactionType = await _context.TransactionTypes
                .FirstOrDefaultAsync(tt => tt.Name == transactionDto.TransactionType);

            if (transactionType == null)
            {
                throw new KeyNotFoundException($"Transaction type '{transactionDto.TransactionType}' not found.");
            }

            var transaction = _mapper.Map<BankAccountTransaction>(transactionDto);
            transaction.Id = Guid.NewGuid();
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = transactionType;

            _context.BankAccountTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task AddRequisitesTransferAsync(BankAccountTransactionDTO transactionDto)
        {
            if (string.IsNullOrWhiteSpace(transactionDto.Mfo) || string.IsNullOrWhiteSpace(transactionDto.AccountNumber))
            {
                throw new ArgumentException("MFO and Account Number are required for this transfer.");
            }

            var transactionType = await _context.TransactionTypes
                .FirstOrDefaultAsync(tt => tt.Name == transactionDto.TransactionType);

            if (transactionType == null)
            {
                throw new KeyNotFoundException($"Transaction type '{transactionDto.TransactionType}' not found.");
            }

            var transaction = _mapper.Map<BankAccountTransaction>(transactionDto);
            transaction.Id = Guid.NewGuid();
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = transactionType;

            _context.BankAccountTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
