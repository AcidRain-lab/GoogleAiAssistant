using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class PhoneTypeService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public PhoneTypeService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PhoneTypeDTO>> GetAllAsync()
        {
            var phoneTypes = await _context.PhoneTypes.ToListAsync();
            return _mapper.Map<IEnumerable<PhoneTypeDTO>>(phoneTypes);
        }

        public async Task<PhoneTypeDTO> GetByIdAsync(int id)
        {
            var phoneType = await _context.PhoneTypes.FindAsync(id);
            return _mapper.Map<PhoneTypeDTO>(phoneType);
        }

        public async Task CreateAsync(PhoneTypeDTO dto)
        {
            var phoneType = _mapper.Map<PhoneType>(dto);
            _context.PhoneTypes.Add(phoneType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhoneTypeDTO dto)
        {
            var phoneType = _mapper.Map<PhoneType>(dto);
            _context.PhoneTypes.Update(phoneType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var phoneType = await _context.PhoneTypes.FindAsync(id);
            if (phoneType != null)
            {
                _context.PhoneTypes.Remove(phoneType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
