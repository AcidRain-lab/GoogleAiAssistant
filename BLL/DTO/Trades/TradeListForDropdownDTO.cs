namespace BLL.DTO.Trades
{
    public class TradeListForDropdownDTO
    {
        public Guid TradeId { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
