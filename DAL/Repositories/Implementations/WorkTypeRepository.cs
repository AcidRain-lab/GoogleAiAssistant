using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class WorkTypeRepository : IWorkTypeRepository
    {
        public readonly CrmContext _context;
        public WorkTypeRepository(CrmContext context)
        {
                _context = context;
        }
        public async Task<List<WorkType>> GetWorkTypeList()
        {
            var workTypeList = await _context.WorkTypes.ToListAsync().ConfigureAwait(false);
            return workTypeList;
        }
    }
}
