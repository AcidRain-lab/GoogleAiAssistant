
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class TradeTypeRepository : ITradeTypeRepository
    {
        public readonly CrmContext _context;
        public TradeTypeRepository(CrmContext context)
        {
            _context = context;
        }
        public async Task<List<TradeType>> GetTradeTypeList()
        {
            var tradeTypeList = await  _context.TradeTypes.ToListAsync().ConfigureAwait(false);
            return tradeTypeList;
        }
    }
}
