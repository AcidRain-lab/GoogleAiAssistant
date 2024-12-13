using BLL.DTO.Common;
using BLL.DTO.Estimates;
using BLL.DTO.EstimateSections;

namespace BLL.Services
{
    public interface IEstimateManager
    {
        Task<ServiceResult> EditCreateEstimate(EstimatesInputDTO input);
        Task<List<EstimateSummaryDTO>> GetEstimatesList();
        Task<EstimatesInputDTO> GetEstimateById(Guid id);
        Task<ServiceResult> DeleteEstimate(Guid id);
        Task<EstimateSummaryDTO> CreateEstimateSection(EstimateTradesInputDTO input);
        Task<EstimateSectionInputDTO> GetEstimateSectionBySectionId(Guid sectionId);
    }
}
