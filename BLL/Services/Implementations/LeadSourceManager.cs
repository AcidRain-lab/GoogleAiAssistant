
using BLL.DTO.LeadSources;
using DAL.Repositories;
namespace BLL.Services.Implementations
{
    public class LeadSourceManager : ILeadSourceManager
    {
        private readonly ILeadSourceRepository _leadSourceRepository;
        public LeadSourceManager(ILeadSourceRepository leadSourceRepository)
        {
            _leadSourceRepository = leadSourceRepository;
        }
        public async Task<List<LeadSourceDTO>> GetLeadSourcesList()
        {
            var leadSourceList = await _leadSourceRepository.GetLeadSourcesList();
            List<LeadSourceDTO> leadSources = new();
            if (leadSourceList.Any())
            {
                leadSourceList.ForEach(leadSource =>
                {
                    LeadSourceDTO leadSourceDTO = new()
                    {
                        LeadSourceId = leadSource.Id,
                        Name = leadSource.Name ?? ""
                    };
                    leadSources.Add(leadSourceDTO);
                });
            }
            return leadSources;
        }
    }
}
