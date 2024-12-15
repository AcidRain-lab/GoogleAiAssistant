using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class IndividualService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public IndividualService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IndividualDTO>> GetAllAsync()
        {
            var individuals = await _context.Individuals.ToListAsync();
            return _mapper.Map<IEnumerable<IndividualDTO>>(individuals);
        }

        public async Task<IndividualDTO?> GetByIdAsync(Guid id)
        {
            var individual = await _context.Individuals.FindAsync(id);
            return individual == null ? null : _mapper.Map<IndividualDTO>(individual);
        }

        public async Task CreateAsync(IndividualDTO dto)
        {
            var individual = _mapper.Map<Individual>(dto);
            _context.Individuals.Add(individual);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IndividualDTO dto)
        {
            var individual = _mapper.Map<Individual>(dto);
            _context.Individuals.Update(individual);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var individual = await _context.Individuals.FindAsync(id);
            if (individual != null)
            {
                _context.Individuals.Remove(individual);
                await _context.SaveChangesAsync();
            }
        }
    }
}
