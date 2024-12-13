using BLL.DTO.States;

namespace BLL.Services
{
    public interface IStateManager
    {
        Task<List<StateDTO>> GetStatesList();
    }
}
