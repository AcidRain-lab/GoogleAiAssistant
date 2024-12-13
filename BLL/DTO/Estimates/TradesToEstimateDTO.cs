namespace BLL.DTO.Estimates
{
    public class TradesToEstimateDTO
    {
        public Guid Id { get; set; }
        public List<Guid> TradeIds { get; set; } = new();
    }
}
