using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class DepositService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public DepositService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepositDTO>> GetByClientIdAsync(Guid clientId)
        {
            var deposits = await _context.Deposits
                .Where(d => d.ClientId == clientId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<DepositDTO>>(deposits);
        }

        public async Task<DepositDTO?> GetByIdAsync(Guid id)
        {
            var deposit = await _context.Deposits.FirstOrDefaultAsync(d => d.Id == id);
            return deposit == null ? null : _mapper.Map<DepositDTO>(deposit);
        }

        public async Task CreateAsync(DepositDTO depositDto)
        {
            var deposit = _mapper.Map<Deposit>(depositDto);
            deposit.Id = Guid.NewGuid();
            _context.Deposits.Add(deposit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DepositDTO depositDto)
        {
            var deposit = await _context.Deposits.FirstOrDefaultAsync(d => d.Id == depositDto.Id);
            if (deposit == null)
                throw new KeyNotFoundException($"Deposit with ID {depositDto.Id} not found.");

            _mapper.Map(depositDto, deposit);
            _context.Deposits.Update(deposit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var deposit = await _context.Deposits.FirstOrDefaultAsync(d => d.Id == id);
            if (deposit == null)
                throw new KeyNotFoundException($"Deposit with ID {id} not found.");

            _context.Deposits.Remove(deposit);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ClientExistsAsync(Guid clientId)
        {
            return await _context.Clients.AnyAsync(c => c.Id == clientId);
        }
    }
}
