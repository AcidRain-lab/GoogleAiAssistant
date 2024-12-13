using BLL.DTO.Templates;
using BLL.DTO.Common;

namespace BLL.Services
{
    public interface ITemplatesManager
    {
        Task<List<TemplateDTO>> GetTemplatesList();

        Task<List<TemplateDTO>> GetTemplatesList(Guid id);
        Task<ServiceResult> EditCreateTemplate(TemplatesInputDTO input);
        Task<TemplatesInputDTO> GetTemplateById(Guid id);
        Task<ServiceResult> DeleteTemplate(Guid id);
        Task<List<TemplateDTO>> GetTemplateListByIds(List<Guid> ids);
    }
}
