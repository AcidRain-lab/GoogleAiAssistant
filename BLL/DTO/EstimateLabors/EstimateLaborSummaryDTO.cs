namespace BLL.DTO.EstimateLabors
{
    public class EstimateLaborSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public decimal? Quantity { get; set; }

        public decimal? Waste { get; set; }

        public decimal? Measurement { get; set; }

        public decimal? CostUnit { get; set; }

        public decimal? PriceUnit { get; set; }

        public bool? UsePriceUnit { get; set; }
        public decimal? TotalCost { get; set; }
        // Method to calculate total cost based on quantity and cost
        public void CalculateTotalCost()
        {
            TotalCost = Quantity * CostUnit;
        }
    }
}
