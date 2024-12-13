using BLL.DTO;
using BLL.DTO.Leads;

namespace BLL.Services
{
    public interface ILeadManager
    {
        Task<List<LeadListForDropdownDTO>> GetAllLeadsList();
        Task<LeadSummaryDTO?> GetLeadByGuidId(Guid id);
        Task<bool> EmailExists(string email);
        Task<LeadDTO> GetLeadDataForEditById(Guid id);
    }
}
