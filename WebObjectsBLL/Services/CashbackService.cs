using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class CashbackService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public CashbackService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CashbackDTO>> GetByClientIdAsync(Guid clientId)
        {
            var cashbacks = await _context.Cashbacks
                .Where(c => c.ClientId == clientId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CashbackDTO>>(cashbacks);
        }

        public async Task<CashbackDTO?> GetByIdAsync(Guid id)
        {
            var cashback = await _context.Cashbacks.FirstOrDefaultAsync(c => c.Id == id);
            return cashback == null ? null : _mapper.Map<CashbackDTO>(cashback);
        }

        public async Task CreateAsync(CashbackDTO cashbackDto)
        {
            var cashback = _mapper.Map<Cashback>(cashbackDto);
            cashback.Id = Guid.NewGuid();
            _context.Cashbacks.Add(cashback);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CashbackDTO cashbackDto)
        {
            var cashback = await _context.Cashbacks.FirstOrDefaultAsync(c => c.Id == cashbackDto.Id);
            if (cashback == null)
                throw new KeyNotFoundException($"Cashback with ID {cashbackDto.Id} not found.");

            _mapper.Map(cashbackDto, cashback);
            _context.Cashbacks.Update(cashback);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var cashback = await _context.Cashbacks.FirstOrDefaultAsync(c => c.Id == id);
            if (cashback == null)
                throw new KeyNotFoundException($"Cashback with ID {id} not found.");

            _context.Cashbacks.Remove(cashback);
            await _context.SaveChangesAsync();
        }
    }
}
