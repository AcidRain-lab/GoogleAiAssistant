using BLL.DTO.SectionItems;

namespace BLL.DTO.EstimateSections
{
    public class EstimateSectionInputDTO
    {
        public Guid SectionId { get; set; } = Guid.Empty;
        public string SectionName { get; set; } = string.Empty;
        public Guid EstimateId { get; set; } = Guid.Empty;
        public List<SectionItemsSummaryDTO> SectionItems { get; set; } = new();
    }
}
