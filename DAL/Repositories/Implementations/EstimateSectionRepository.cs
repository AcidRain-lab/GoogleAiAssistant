using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class EstimateSectionRepository : IEstimateSectionRepository
    {
        private readonly CrmContext _context;
        public EstimateSectionRepository(CrmContext context)
        {
            _context = context;
        }
        public async Task<EstimateSection> GetById(Guid id)
        {
            var estimateSection = await _context.EstimateSections.Include(x => x.SectionsItems).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            return estimateSection!;
        }
    }
}
