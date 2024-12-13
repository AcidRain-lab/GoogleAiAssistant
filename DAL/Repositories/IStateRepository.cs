using DAL.Models;

namespace DAL.Repositories
{
    public interface IStateRepository
    {
        Task<List<State>> GetStatesList();
    }
}
