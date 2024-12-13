using DAL.Models;

namespace DAL.Repositories.Implementations
{
    public class SectionItemRepository : ISectionItemRepository
    {
        private readonly CrmContext _context;
        public SectionItemRepository(CrmContext context)
        {
            _context = context;
        }
        public async Task<int> SaveRange(List<SectionsItem> estimateTemp)
        {
            await _context.AddRangeAsync(estimateTemp);
            return await _context.SaveChangesAsync();
        }
    }
}
