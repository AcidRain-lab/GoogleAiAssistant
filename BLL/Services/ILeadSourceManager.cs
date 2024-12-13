
using BLL.DTO.LeadSources;

namespace BLL.Services
{
    public interface ILeadSourceManager
    {
        Task<List<LeadSourceDTO>> GetLeadSourcesList();
    }
}
