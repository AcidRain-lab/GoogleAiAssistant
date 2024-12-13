using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class PhoneService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public PhoneService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PhoneDTO>> GetAllAsync()
        {
            var phones = await _context.Phones.ToListAsync();
            return _mapper.Map<IEnumerable<PhoneDTO>>(phones);
        }

        public async Task CreateAsync(PhoneDTO dto)
        {
            var phone = _mapper.Map<Phone>(dto);
            _context.Phones.Add(phone);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhoneDTO dto)
        {
            var phone = _mapper.Map<Phone>(dto);
            _context.Phones.Update(phone);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone != null)
            {
                _context.Phones.Remove(phone);
                await _context.SaveChangesAsync();
            }
        }
    }
}
