using DAL.Models;

namespace DAL.Repositories
{
    public interface ITradeRepository
    {
        Task<int> Save(Trade trade);
        Task<int> Update(Trade trade);
        Task<Trade> GetById(Guid id);
        Task<List<Trade>> GetAll();
        Task<int> Delete(Trade trade);
        Task<List<Trade>> GetAllByIds(List<Guid> ids);
    }
}
