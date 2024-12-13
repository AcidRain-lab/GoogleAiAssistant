
using BLL.DTO.States;
using DAL.Repositories;

namespace BLL.Services.Implementations
{
   
    public class StateManager : IStateManager
    {
        private readonly IStateRepository _stateRepository;
        public StateManager(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<List<StateDTO>> GetStatesList()
        {
            var statesList = await _stateRepository.GetStatesList();
            List<StateDTO> states = new List<StateDTO>();
            if(statesList.Any())
            {
                statesList.ForEach(state => 
                { 
                    StateDTO stateDTO = new()
                    {
                        StateId = state.Id,
                        Name = state.Name,
                    };
                    states.Add(stateDTO);
                });
            }
            return states;
        }
    }
}
