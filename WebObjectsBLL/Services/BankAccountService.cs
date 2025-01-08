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

        public async Task<BankAccountDTO?> GetAccountDetailsAsync(Guid accountId)
        {
            var account = await _context.BankAccounts
                .Include(a => a.BankCards).ThenInclude(c => c.CardType)
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null) return null;

            return new BankAccountDTO
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                AccountName = account.AccountName,
                BankAccountTypeId = account.BankAccountTypeId,
                ClientId = account.ClientId,
                OpenedDate = account.OpenedDate.ToDateTime(new TimeOnly()),
                ClosedDate = account.ClosedDate?.ToDateTime(new TimeOnly()),
                ContractTerms = account.ContractTerms,
                Balance = account.Balance,
                BankCurrencyId = account.BankCurrencyId,
                IsFop = account.IsFop,
                BankCards = account.BankCards.Select(card => new BankCardDTO
                {
                    Id = card.Id,
                    CardNumber = card.CardNumber,
                    CardTypeName = card.CardType.Name,
                    CreditLimit = card.CreditLimit
                }).ToList(),
                Client = new ClientDTO
                {
                    Id = account.Client.Id,
                    Email = account.Client.Email,
                    Phone = account.Client.Phone
                },
                ClientFullName = $"{account.Client.FirstName} {account.Client.LastName}"
            };
        }



    }
}
