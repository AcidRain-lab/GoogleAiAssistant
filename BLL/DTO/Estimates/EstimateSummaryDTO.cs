namespace BLL.DTO.Estimates
{
    public class EstimateSummaryDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public Guid ContactId { get; set; }

        public string? Notes { get; set; }

        public int CalculateTypeId { get; set; }

        public decimal Square { get; set; }

        public int JobCategoryId { get; set; }

        public int WorkTypeId { get; set; }
        public decimal? PriceMaterial { get; set; }

        public decimal? PriceLabor { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? Text { get; set; }
        public string? FullName { get; set; } = string.Empty;
        public Guid SectionId { get; set; }=Guid.Empty;
        public List<Guid> TradeIds { get; set; } = new();
        public string ContactName { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
    }
}
