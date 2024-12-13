using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class JobCategoryRepository : IJobCategoryRepository
    {
        private readonly CrmContext _context;
        public JobCategoryRepository(CrmContext context)
        {
            _context = context;
        }

        public async Task<List<JobCategory>> GetJobCategoriesList()
        {
            var jobCategoryList = await _context.JobCategories.ToListAsync().ConfigureAwait(false);
            return jobCategoryList;
        }
    }
}
