using DAL.Models;

namespace DAL.Repositories
{
    public interface ILeadSourceRepository
    {
        Task<List<LeadSource>> GetLeadSourcesList();
    }
}
