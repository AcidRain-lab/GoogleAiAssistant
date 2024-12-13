
using DAL.Models;

namespace DAL.Repositories
{
    public interface ISectionItemRepository
    {
        Task<int> SaveRange(List<SectionsItem> estimateTemp);
    }
}
