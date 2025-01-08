using DAL.Models;
using WebObjectsBLL.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WebObjectsBLL.Services
{
    public class DepositTermService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public DepositTermService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DepositTermDTO>> GetByDepositTypeIdAsync(Guid depositTypeId)
        {
            var terms = await _context.DepositTerms
                .Where(t => t.DepositTypeId == depositTypeId)
                .ToListAsync();

            return _mapper.Map<List<DepositTermDTO>>(terms);
        }

        public async Task<DepositTermDTO> GetByIdAsync(Guid id)
        {
            var term = await _context.DepositTerms.FindAsync(id);
            if (term == null) throw new KeyNotFoundException("Deposit term not found.");
            return _mapper.Map<DepositTermDTO>(term);
        }

        public async Task AddAsync(DepositTermDTO termDto)
        {
            var term = _mapper.Map<DepositTerm>(termDto);
            _context.DepositTerms.Add(term);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DepositTermDTO termDto)
        {
            var term = await _context.DepositTerms.FindAsync(termDto.Id);
            if (term == null) throw new KeyNotFoundException("Deposit term not found.");
            _mapper.Map(termDto, term);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var term = await _context.DepositTerms.FindAsync(id);
            if (term == null) throw new KeyNotFoundException("Deposit term not found.");
            _context.DepositTerms.Remove(term);
            await _context.SaveChangesAsync();
        }
    }
}
