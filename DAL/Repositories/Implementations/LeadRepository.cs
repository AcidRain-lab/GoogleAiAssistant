using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class LeadRepository : ILeadRepository
    {
        private readonly CrmContext _context;
        public LeadRepository(CrmContext context)
        {
            _context = context;
        }
        public async Task<List<Lead>> GetLeadsList()
        {
            var leadList = await _context.Leads.ToListAsync().ConfigureAwait(false);
            return leadList;
        }

        public async Task<Lead?> GetLeadById(Guid id)
        {
            return (await _context.Leads.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false));
        }
        public async Task<bool> EmailExists(string email) => await _context.Leads.AnyAsync(x => x.Email == email).ConfigureAwait(false);

    }
}
