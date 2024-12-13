
using DAL.Models;
namespace DAL.Repositories
{
    public interface ILocationRepository
    {
        Task<Location?> GetLocationById(Guid id);
        Task<Guid> Save(Location location);
    }
}
