namespace BLL.DTO.EstimateMaterials
{
    public class EstimateMaterialSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public decimal? Quantity { get; set; }
        public decimal? Cost { get; set; }
        public decimal? TotalCost { get; set; }
        // Method to calculate total cost based on quantity and cost
        public void CalculateTotalCost()
        {
            TotalCost = Quantity * Cost;
        }
    }
}
