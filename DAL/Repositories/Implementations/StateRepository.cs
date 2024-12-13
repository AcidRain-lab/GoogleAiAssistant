using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class StateRepository : IStateRepository
    {
        private readonly CrmContext _context;
        public StateRepository(CrmContext context)
        {
            _context = context;
        }

        public async Task<List<State>> GetStatesList()
        {
            var statesList = await _context.States.ToListAsync().ConfigureAwait(false);
            return statesList;
        }
    }
}
