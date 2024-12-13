using DAL.Models;

namespace DAL.Repositories
{
    public interface IEstimateRepository
    {
        Task<int> Update(Estimate appointment);
        Task<int> Delete(Estimate appointment);
        Task<int> Save(Estimate appointment);
        Task<Estimate> GetById(Guid id);
        Task<List<Estimate>> GetAll();
        Task<int> CreateEstimateSection(EstimateSection section);
    }
}
