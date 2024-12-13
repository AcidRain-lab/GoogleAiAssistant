using BLL.DTO.EstimateLabors;
using BLL.DTO.EstimateMaterials;

namespace BLL.DTO.SectionItems
{
    public class SectionItemsSummaryDTO
    {
        public string ItemName { get; set; } = string.Empty;
        public Guid ItemId { get; set; }
        public string TemplateText { get; set; } = string.Empty;
        public decimal? LaborTotalCost { get; set; }
        public decimal? MaterialTotalCost { get; set; }
        public List<EstimateMaterialSummaryDTO> EstimateMaterialList { get; set; } = new();
        public List<EstimateLaborSummaryDTO> EstimateLaborList { get; set; } = new();
    }
}
