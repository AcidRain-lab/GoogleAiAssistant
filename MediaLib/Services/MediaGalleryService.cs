using DAL.Models;
using MediaLib.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MediaLib.Services
{
    public class MediaGalleryService
    {
        private readonly BankContext _context;

        public MediaGalleryService(BankContext context)
        {
            _context = context;
        }

        public async Task<List<MediaDataDTO>> GetMediaDataListAsync(Guid recordId)
        {
            var mediaList = await _context.MediaData
                .Where(m => m.AssociatedRecordId == recordId)
                .ToListAsync();

            return mediaList.Select(m => new MediaDataDTO
            {
                Id = m.Id,
                Name = m.Name,
                Extension = m.Extension,
                Content = m.Content,
                AssociatedRecordId = m.AssociatedRecordId,
                ObjectTypeId = m.ObjectTypeId,
                IsPrime = m.IsPrime,
                Base64Image = m.Content != null ? Convert.ToBase64String(m.Content) : null
            }).ToList();
        }

        public async Task AddMediaAsync(List<MediaDataDTO> mediaFiles)
        {
            foreach (var media in mediaFiles)
            {
                var newMedia = new MediaDatum
                {
                    Id = Guid.NewGuid(),
                    Name = media.Name,
                    Extension = media.Extension,
                    Content = media.Content,
                    AssociatedRecordId = media.AssociatedRecordId,
                    ObjectTypeId = media.ObjectTypeId,
                    IsPrime = media.IsPrime
                };
                await _context.MediaData.AddAsync(newMedia);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateMediaAsync(List<MediaDataDTO> mediaFiles)
        {
            foreach (var media in mediaFiles)
            {
                var existingMedia = await _context.MediaData.FindAsync(media.Id);
                if (existingMedia != null)
                {
                    existingMedia.Name = media.Name;
                    existingMedia.Extension = media.Extension;
                    existingMedia.Content = media.Content;
                    existingMedia.IsPrime = media.IsPrime;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveMediaByRecordIdAsync(Guid recordId)
        {
            var mediaFiles = await _context.MediaData
                .Where(m => m.AssociatedRecordId == recordId)
                .ToListAsync();

            if (!mediaFiles.Any()) return false;

            _context.MediaData.RemoveRange(mediaFiles);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveMediaAsync(Guid mediaId)
        {
            var media = await _context.MediaData.FindAsync(mediaId);
            if (media == null) return false;

            _context.MediaData.Remove(media);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
