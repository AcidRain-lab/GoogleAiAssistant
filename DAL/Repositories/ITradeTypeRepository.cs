using DAL.Models;

namespace DAL.Repositories
{
    public interface ITradeTypeRepository
    {
        Task<List<TradeType>> GetTradeTypeList();
    }
}
