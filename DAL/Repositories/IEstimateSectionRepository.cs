using DAL.Models;

namespace DAL.Repositories
{
    public interface IEstimateSectionRepository
    {
        Task<EstimateSection> GetById(Guid id);
    }
}
