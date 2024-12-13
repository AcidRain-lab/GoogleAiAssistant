
using DAL.Models;

namespace DAL.Repositories
{
    public interface ILeadRepository
    {
        Task<List<Lead>> GetLeadsList();
        Task<Lead?> GetLeadById(Guid id);
        Task<bool> EmailExists(string email);
   
    }
}
