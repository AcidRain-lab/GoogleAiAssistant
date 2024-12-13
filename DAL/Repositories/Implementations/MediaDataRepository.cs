
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class MediaDataRepository : IMediaDataRepository
    {
        private readonly CrmContext _context;
        public MediaDataRepository(CrmContext context)
        {
            _context = context;
        }

        public async Task<List<MediaDatum>> GetMediaDataListByAssociatedRecordId(Guid associatedRecordId)
        {
            var mediaDataList = await _context.MediaData.Where(x => x.AssociatedRecordId == associatedRecordId).ToListAsync().ConfigureAwait(false);
            return mediaDataList;
        }
        public async Task<bool> SaveMediaDataList(List<MediaDatum> inputList)
        {
            foreach (var mediaData in inputList)
            {
                await _context.MediaData.AddRangeAsync(mediaData);
            }
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
