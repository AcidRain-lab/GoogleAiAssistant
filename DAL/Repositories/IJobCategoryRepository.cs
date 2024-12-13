
using DAL.Models;

namespace DAL.Repositories
{
    public interface IJobCategoryRepository
    {
        Task<List<JobCategory>> GetJobCategoriesList();
    }
}
