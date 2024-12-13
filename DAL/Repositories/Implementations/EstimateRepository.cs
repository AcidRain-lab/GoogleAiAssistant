using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class EstimateRepository : IEstimateRepository
    {
        private readonly CrmContext _context;

        public EstimateRepository(CrmContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(Estimate estimate)
        {
            _context.Estimates.Remove(estimate);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Estimate>> GetAll()
        {
            var estimates = await _context.Estimates.ToListAsync().ConfigureAwait(false);
            return estimates;
        }

        public async Task<Estimate> GetById(Guid id)
        {
            var estimate = await _context.Estimates.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            return estimate!;
        }

        public async Task<int> Save(Estimate estimate)
        {

            await _context.AddAsync(estimate);
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> Update(Estimate estimate)
        {
            _context.Update(estimate);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> CreateEstimateSection(EstimateSection section)
        {
            await _context.AddAsync(section);
            return await _context.SaveChangesAsync();
        }
    }
}
