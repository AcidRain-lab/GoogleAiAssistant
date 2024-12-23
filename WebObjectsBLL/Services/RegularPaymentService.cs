using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class RegularPaymentService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public RegularPaymentService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegularPaymentDTO>> GetByClientIdAsync(Guid clientId)
        {
            var payments = await _context.RegularPayments
                .Where(p => p.ClientId == clientId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RegularPaymentDTO>>(payments);
        }

        public async Task<RegularPaymentDTO?> GetByIdAsync(Guid id)
        {
            var payment = await _context.RegularPayments.FirstOrDefaultAsync(p => p.Id == id);
            return payment == null ? null : _mapper.Map<RegularPaymentDTO>(payment);
        }

        public async Task CreateAsync(RegularPaymentDTO paymentDto)
        {
            var payment = _mapper.Map<RegularPayment>(paymentDto);
            payment.Id = Guid.NewGuid();
            _context.RegularPayments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RegularPaymentDTO paymentDto)
        {
            var payment = await _context.RegularPayments.FirstOrDefaultAsync(p => p.Id == paymentDto.Id);
            if (payment == null)
                throw new KeyNotFoundException($"RegularPayment with ID {paymentDto.Id} not found.");

            _mapper.Map(paymentDto, payment);
            _context.RegularPayments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var payment = await _context.RegularPayments.FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null)
                throw new KeyNotFoundException($"RegularPayment with ID {id} not found.");

            _context.RegularPayments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }
}
