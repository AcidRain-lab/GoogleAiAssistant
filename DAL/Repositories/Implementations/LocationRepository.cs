using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly CrmContext _context;
        public LocationRepository(CrmContext context)
        {
                _context = context;
        }
        public async Task<Location?> GetLocationById(Guid id)
        {
            var locationResult = await _context.Locations.Where(loc => loc.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
            return locationResult;
        }
        public async Task<Guid> Save(Location location)
        {
            await _context.AddAsync(location).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            if (location != null)
            {
                return location.Id;
            }
            return Guid.Empty;
        }

    }
}
