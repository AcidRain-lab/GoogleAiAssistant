using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class CountryService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public CountryService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryDTO>> GetAllAsync()
        {
            var countries = await _context.Countries.ToListAsync();
            return _mapper.Map<IEnumerable<CountryDTO>>(countries);
        }

        public async Task<CountryDTO> GetByIdAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            return _mapper.Map<CountryDTO>(country);
        }

        public async Task CreateAsync(CountryDTO dto)
        {
            var country = _mapper.Map<Country>(dto);
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CountryDTO dto)
        {
            var country = _mapper.Map<Country>(dto);
            _context.Countries.Update(country);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();
            }
        }
    }
}
