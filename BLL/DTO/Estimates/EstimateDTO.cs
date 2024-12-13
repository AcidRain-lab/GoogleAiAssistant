namespace BLL.DTO
{
    public class EstimateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public decimal Square { get; set; }
        public decimal? PriceMaterial { get; set; }
        public decimal? PriceLabor { get; set; }
        public decimal? TotalPrice { get; set; }
        public Guid ContactId { get; set; }
        public List<EstimateSectionDTO> EstimateSections { get; set; } = new List<EstimateSectionDTO>();
    }

    public class EstimateSectionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SectionItemDTO> SectionItems { get; set; } = new List<SectionItemDTO>();
    }

    public class SectionItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<EstimateMaterialDTO> Materials { get; set; } = new List<EstimateMaterialDTO>();
        public List<EstimateLaborDTO> Labors { get; set; } = new List<EstimateLaborDTO>();
    }

    public class EstimateMaterialDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Cost { get; set; }
    }

    public class EstimateLaborDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? CostUnit { get; set; }
    }
}
