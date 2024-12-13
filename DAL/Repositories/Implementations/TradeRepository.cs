using DAL.Models;
using Microsoft.EntityFrameworkCore;
namespace DAL.Repositories.Implementations
{
    public class TradeRepository : ITradeRepository
    {
        private readonly CrmContext _context;
        public TradeRepository(CrmContext context)
        {

            _context = context;

        }
        public async Task<int> Save(Trade trade)
        {

            await _context.AddAsync(trade);
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> Update(Trade trade)
        {
            _context.Update(trade);
            return await _context.SaveChangesAsync();
        }
        public async Task<Trade> GetById(Guid id)
        {
            var trade = await _context.Trades.Include(x => x.TradeTemplates).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            return trade!;
        }
        public async Task<List<Trade>> GetAll()
        {
            var trades = await _context.Trades.ToListAsync().ConfigureAwait(false);
            return trades;
        }
        public async Task<int> Delete(Trade trade)
        {
            _context.Trades.Remove(trade);
            return await _context.SaveChangesAsync();
        }
        public async Task<List<Trade>> GetAllByIds(List<Guid> ids)
        {
            var trades = await _context.Trades.Include(x => x.TradeTemplates).Where(x => ids.Contains(x.Id)).ToListAsync().ConfigureAwait(false);
            return trades;
        }
    }
}
