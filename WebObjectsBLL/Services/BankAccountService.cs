// BankAccountService.cs
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
    public class BankAccountService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public BankAccountService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BankAccountDTO>> GetByClientIdAsync(Guid clientId)
        {
            var accounts = await _context.BankAccounts
                .Include(a => a.BankAccountType)
                .Where(a => a.ClientId == clientId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BankAccountDTO>>(accounts);
        }

        public async Task<BankAccountDTO> GetByIdAsync(Guid id)
        {
            var account = await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
                throw new KeyNotFoundException("Bank account not found");

            return _mapper.Map<BankAccountDTO>(account);
        }

        public async Task<BankAccountDTO> CreateForClientAsync(Guid clientId, int bankAccountTypeId, int currencyId, string accountName)
        {
            // Генерация уникального номера счета
            var accountNumber = GenerateAccountNumber();

            // Создание объекта BankAccount
            var newAccount = new BankAccount
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                BankAccountTypeId = bankAccountTypeId,
                BankCurrencyId = currencyId,
                AccountName = accountName,
                AccountNumber = accountNumber,
                OpenedDate = DateOnly.FromDateTime(DateTime.Now),
                Balance = 0, // Начальный баланс
                IsFop = false // Укажите FOP, если необходимо
            };

            // Добавление счета в базу данных
            _context.BankAccounts.Add(newAccount);
            await _context.SaveChangesAsync();

            // Возврат созданного объекта в виде DTO
            return _mapper.Map<BankAccountDTO>(newAccount);
        }

        public async Task UpdateAsync(BankAccountDTO accountDto)
        {
            var account = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == accountDto.Id);
            if (account == null)
                throw new KeyNotFoundException("Bank account not found");

            _mapper.Map(accountDto, account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var account = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == id);
            if (account == null)
                throw new KeyNotFoundException("Bank account not found");

            _context.BankAccounts.Remove(account);
            await _context.SaveChangesAsync();
        }

        private string GenerateAccountNumber()
        {
            return $"UA{new Random().Next(100000000, 999999999)}";
        }

        public async Task<string> GetClientNameAsync(Guid clientId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            return client != null ? $"{client.FirstName} {client.LastName}" : "Unknown Client";
        }
    }
}
