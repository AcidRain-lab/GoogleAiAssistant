using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class LeadSourceRepository : ILeadSourceRepository
    {
        public readonly CrmContext _context;
        public LeadSourceRepository(CrmContext context)
        {
            _context = context;
        }
        public async Task<List<LeadSource>> GetLeadSourcesList()
        {
            var leadSourceList = await _context.LeadSources.ToListAsync().ConfigureAwait(false);
            return leadSourceList;
        }
    }
}
