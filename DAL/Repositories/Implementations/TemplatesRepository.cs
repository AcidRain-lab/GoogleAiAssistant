using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class TemplatesRepository : ITemplatesRepository
    {
        private readonly CrmContext _context;
        public TemplatesRepository(CrmContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TradeTemplate template)
        {
            _context.TradeTemplates.Remove(template);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<TradeTemplate>> GetAll()
        {
            var templates = await _context.TradeTemplates.Include(x => x.Trade).ToListAsync().ConfigureAwait(false);
            return templates;
        }

        public async Task<List<TradeTemplate>> GetAllByTrade(Guid id)
        {
            var templates = await _context.TradeTemplates.Include(x => x.Trade).Where(x=>x.TradeId == id).ToListAsync().ConfigureAwait(false);
            return templates;
        }

        public async Task<TradeTemplate> GetById(Guid id)
        {
            var template = await _context.TradeTemplates.Include(x => x.Trade).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            return template!;
        }

        public async Task<int> Save(TradeTemplate template)
        {

            await _context.AddAsync(template);
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> Update(TradeTemplate template)
        {
            _context.Update(template);
            return await _context.SaveChangesAsync();
        }
        public async Task<List<TradeTemplate>> GetAllByIds(List<Guid> ids)
        {
            var templates = await _context.TradeTemplates.Where(x => ids.Contains(x.Id)).ToListAsync().ConfigureAwait(false);
            return templates;
        }

    }
}
