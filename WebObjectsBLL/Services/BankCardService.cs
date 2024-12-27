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
    public class BankCardService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public BankCardService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BankCardDTO>> GetByClientIdAsync(Guid clientId)
        {
            var cards = await _context.BankCards
                .Include(c => c.CardType)
                .Include(c => c.BankAccount)
                .Where(c => c.BankAccount.ClientId == clientId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BankCardDTO>>(cards);
        }

        public async Task<BankCardDTO> GetByIdAsync(Guid id)
        {
            var card = await _context.BankCards
                .Include(c => c.CardType)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (card == null)
                throw new KeyNotFoundException("BankCard not found");

            return _mapper.Map<BankCardDTO>(card);
        }

        public async Task CreateAsync(Guid clientId, Guid cardTypeId, string cardHolderName)
        {
            // Создаем новый банковский аккаунт для клиента
            var newAccount = new BankAccount
            {
                Id = Guid.NewGuid(),
                AccountNumber = GenerateAccountNumber(),
                AccountName = $"{cardHolderName}'s Account",
                ClientId = clientId,
                OpenedDate = DateOnly.FromDateTime(DateTime.Now),
                Balance = 0m,
                BankAccountTypeId = 1, // Тип аккаунта (обычный)
                BankCurrencyId = 1,    // Валюта (например, USD)
                IsFop = false          // Указать, если не для предпринимателя
            };

            await _context.BankAccounts.AddAsync(newAccount);
            await _context.SaveChangesAsync();

            // Создаем новую карту, привязанную к созданному аккаунту
            var newCard = new BankCard
            {
                Id = Guid.NewGuid(),
                BankAccountId = newAccount.Id,
                CardTypeId = cardTypeId,
                CardHolderName = cardHolderName,
                CardNumber = GenerateCardNumber(),
                ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(3)),
                PinCode = GeneratePinCode(),
                Cvv = GenerateCvv(),
                IsActive = true,
                IsPrimary = false
            };

            await _context.BankCards.AddAsync(newCard);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var card = await _context.BankCards.FirstOrDefaultAsync(c => c.Id == id);
            if (card == null)
                throw new KeyNotFoundException("BankCard not found");

            _context.BankCards.Remove(card);
            await _context.SaveChangesAsync();
        }

        private string GenerateAccountNumber()
        {
            return $"UA{new Random().Next(100000000, 999999999)}";
        }

        private string GenerateCardNumber()
        {
            return $"4000{new Random().Next(100000000, 999999999)}";
        }

        private string GeneratePinCode()
        {
            return new Random().Next(1000, 9999).ToString();
        }

        private string GenerateCvv()
        {
            return new Random().Next(100, 999).ToString();
        }

        public async Task<IEnumerable<BankCardDTO>> GetByBankAccountIdAsync(Guid bankAccountId)
        {
            var cards = await _context.BankCards
                .Include(c => c.CardType)
                .Where(c => c.BankAccountId == bankAccountId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BankCardDTO>>(cards);
        }
    }
}
