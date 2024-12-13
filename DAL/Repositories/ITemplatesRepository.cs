using DAL.Models;

namespace DAL.Repositories
{
    public interface ITemplatesRepository
    {
        Task<int> Update(TradeTemplate template);
        Task<int> Delete(TradeTemplate template);
        Task<int> Save(TradeTemplate template);
        Task<TradeTemplate> GetById(Guid id);
        Task<List<TradeTemplate>> GetAllByTrade(Guid id);
        Task<List<TradeTemplate>> GetAll();
        Task<List<TradeTemplate>> GetAllByIds(List<Guid> ids);
    }
}
