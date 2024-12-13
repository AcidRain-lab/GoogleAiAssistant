using BLL.DTO;
using BLL.DTO.JobCategories;

namespace BLL.Services
{
    public interface IJobCategoryManager
    {
        Task<List<JobCategorySummaryDTO>> GetJobCategoriesList();
    }
}
