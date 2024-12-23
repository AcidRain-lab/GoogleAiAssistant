using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class CreditService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public CreditService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CreditDTO>> GetByClientIdAsync(Guid clientId)
        {
            var credits = await _context.Credits
                .Where(c => c.ClientId == clientId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CreditDTO>>(credits);
        }

        public async Task<CreditDTO?> GetByIdAsync(Guid id)
        {
            var credit = await _context.Credits.FirstOrDefaultAsync(c => c.Id == id);
            return credit == null ? null : _mapper.Map<CreditDTO>(credit);
        }

        public async Task CreateAsync(CreditDTO creditDto)
        {
            var clientExists = await _context.Clients.AnyAsync(c => c.Id == creditDto.ClientId);
            if (!clientExists)
            {
                throw new KeyNotFoundException($"Client with ID {creditDto.ClientId} not found.");
            }

            var credit = _mapper.Map<Credit>(creditDto);
            credit.Id = Guid.NewGuid();
            _context.Credits.Add(credit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CreditDTO creditDto)
        {
            var credit = await _context.Credits.FirstOrDefaultAsync(c => c.Id == creditDto.Id);
            if (credit == null)
                throw new KeyNotFoundException($"Credit with ID {creditDto.Id} not found.");

            _mapper.Map(creditDto, credit);
            _context.Credits.Update(credit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var credit = await _context.Credits.FirstOrDefaultAsync(c => c.Id == id);
            if (credit == null)
                throw new KeyNotFoundException($"Credit with ID {id} not found.");

            _context.Credits.Remove(credit);
            await _context.SaveChangesAsync();
        }
    }
}
