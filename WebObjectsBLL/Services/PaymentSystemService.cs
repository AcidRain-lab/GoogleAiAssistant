using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class PaymentSystemService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public PaymentSystemService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Получить все платежные системы
        public async Task<IEnumerable<PaymentSystemTypeDTO>> GetAllAsync()
        {
            var paymentSystems = await _context.PaymentSystemTypes.ToListAsync();
            return _mapper.Map<IEnumerable<PaymentSystemTypeDTO>>(paymentSystems);
        }

        // Получить платежную систему по ID
        public async Task<PaymentSystemTypeDTO> GetByIdAsync(int id)
        {
            var paymentSystem = await _context.PaymentSystemTypes.FirstOrDefaultAsync(ps => ps.Id == id);
            if (paymentSystem == null)
                throw new KeyNotFoundException("Payment system not found");

            return _mapper.Map<PaymentSystemTypeDTO>(paymentSystem);
        }
    }
}
