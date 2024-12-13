using BLL.DTO.WorkTypes;
using DAL.Repositories;

namespace BLL.Services.Implementations
{
    public class WorkTypeManager : IWorkTypeManager
    {
        private readonly IWorkTypeRepository _workTypeRepository;
        public WorkTypeManager(IWorkTypeRepository workTypeRepository)
        {
                _workTypeRepository = workTypeRepository;
        }
        public async Task<List<WorkTypeDTO>> GetWorkTypeList()
        {
            var workTypeList = await _workTypeRepository.GetWorkTypeList();
            List<WorkTypeDTO> workTypes = new();
            if (workTypeList.Any())
            {
                workTypeList.ForEach(workType =>
                {
                    WorkTypeDTO workTypeDTO = new()
                    {
                        WorkTypeId = workType.Id,
                        Name = workType.Name ?? ""
                    };
                    workTypes.Add(workTypeDTO);
                });
            }
            return workTypes;
        }
    }
}
