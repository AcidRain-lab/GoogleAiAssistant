using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task CreateAsync(BankAccountDTO accountDto)
        {
            var account = _mapper.Map<BankAccount>(accountDto);
            account.Id = Guid.NewGuid();
            account.OpenedDate = DateOnly.FromDateTime(DateTime.Now); // Преобразование

            _context.BankAccounts.Add(account);
            await _context.SaveChangesAsync();
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
    }
}
