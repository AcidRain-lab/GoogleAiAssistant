using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class OrganizationService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public OrganizationService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrganizationDTO>> GetAllAsync()
        {
            var organizations = await _context.Organizations.ToListAsync();
            return _mapper.Map<IEnumerable<OrganizationDTO>>(organizations);
        }

        public async Task<OrganizationDTO?> GetByIdAsync(Guid id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            return organization == null ? null : _mapper.Map<OrganizationDTO>(organization);
        }

        public async Task CreateAsync(OrganizationDTO dto)
        {
            var organization = _mapper.Map<Organization>(dto);
            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrganizationDTO dto)
        {
            var organization = _mapper.Map<Organization>(dto);
            _context.Organizations.Update(organization);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
                await _context.SaveChangesAsync();
            }
        }
    }
}
