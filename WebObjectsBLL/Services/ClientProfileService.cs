using WebObjectsBLL.DTO;
using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace WebObjectsBLL.Services
{
    public class ClientProfileService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public ClientProfileService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClientProfileDTO?> GetClientProfileAsync(Guid clientId)
        {
            var client = await _context.Clients
                .Include(c => c.BankAccounts)
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
                return null;

            return _mapper.Map<ClientProfileDTO>(client);
        }
    }
}
