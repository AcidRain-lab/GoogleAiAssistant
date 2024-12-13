using BLL.DTO.Trades;

namespace BLL.DTO.Estimates
{
    public class EstimateTradesInputDTO
    {
        public Guid EstimateId { get; set; } = Guid.Empty;
        public List<TradesInputDTO> TradeList { get; set; } = new();
        public bool IsNewRecord { get; set; }
        public List<Guid> TradeIds { get; set; } = new();
        public List<Guid> TradeTemplateIds { get; set; } = new();
        public Guid ContactId { get; set; } = Guid.Empty;
    }
}
