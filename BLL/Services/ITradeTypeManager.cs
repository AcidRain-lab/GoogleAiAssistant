
using BLL.DTO.TradeTypes;

namespace BLL.Services
{
    public interface ITradeTypeManager
    {
        Task<List<TradeTypeDTO>> GetTradeTypeList();
    }
}
