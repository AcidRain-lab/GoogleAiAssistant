using BLL.DTO.WorkTypes;
namespace BLL.Services
{
    public interface IWorkTypeManager
    {
        Task<List<WorkTypeDTO>> GetWorkTypeList();
    }
}
