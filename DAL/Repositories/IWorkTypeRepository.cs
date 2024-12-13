using DAL.Models;

namespace DAL.Repositories
{
    public interface IWorkTypeRepository
    {
        Task<List<WorkType>> GetWorkTypeList();
    }
}
