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

        public async Task<IEnumerable<BankCardDTO>> GetByBankAccountIdAsync(Guid bankAccountId)
        {
            var cards = await _context.BankCards
                .Include(c => c.CardType)
                .Where(c => c.BankAccountId == bankAccountId)
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

        public async Task CreateAsync(BankCardDTO cardDto)
        {
            var card = _mapper.Map<BankCard>(cardDto);
            card.Id = Guid.NewGuid(); // Ensure the ID is generated for new records
            _context.BankCards.Add(card);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BankCardDTO cardDto)
        {
            var card = await _context.BankCards.FirstOrDefaultAsync(c => c.Id == cardDto.Id);
            if (card == null)
                throw new KeyNotFoundException("BankCard not found");

            _mapper.Map(cardDto, card);
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
    }
}
