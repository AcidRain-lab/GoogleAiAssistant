using BLL.DTO.Common;
using BLL.DTO.Templates;
using BLL.DTO.Trades;

namespace BLL.Services
{
    public interface ITradeManager
    {
        Task<ServiceResult> EditCreateTrade(TradesInputDTO input);
        Task<TradesInputDTO> GetTradeById(Guid id);
        Task<List<TradesInputDTO>> GetTradeList();
        Task<ServiceResult> DeleteTrade(Guid id);
        Task<List<TemplateDTO>> GetTradeTemplateListByTradeIds(List<Guid> ids);
    }
}
