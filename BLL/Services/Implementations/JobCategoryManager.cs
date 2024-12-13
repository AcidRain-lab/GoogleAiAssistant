using BLL.DTO;
using BLL.DTO.JobCategories;
using DAL.Repositories;

namespace BLL.Services.Implementations
{
    public class JobCategoryManager : IJobCategoryManager
    {
        private readonly IJobCategoryRepository _jobCategoryRepository;
        public JobCategoryManager(IJobCategoryRepository jobCategoryRepository)
        {
            _jobCategoryRepository = jobCategoryRepository;
        }

        public async Task<List<JobCategorySummaryDTO>> GetJobCategoriesList()
        {
            var jobCategoryList = await _jobCategoryRepository.GetJobCategoriesList();
            List<JobCategorySummaryDTO> jobCategories = new();
            if (jobCategoryList.Any())
            {
                jobCategoryList.ForEach(category =>
                {
                    JobCategorySummaryDTO jobCategory = new();
                    jobCategory.Name = category.Name;
                    jobCategory.JobCategoryId = category.Id;
                    jobCategories.Add(jobCategory);
                });
            }
            return jobCategories;
        }
    }
}
