using DAL.Models;
using MediaLib.DTO;
using MediaLib.DTO.MediaData;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediaLib.Services
{
    public class MediaGalleryService
    {
        private readonly BankContext _context;

        public MediaGalleryService(BankContext context)
        {
            _context = context;
        }

        public async Task<bool> HasMediaAsync(Guid associatedRecordId)
        {
            return await _context.MediaData.AnyAsync(m => m.AssociatedRecordId == associatedRecordId);
        }

        public async Task<List<MediaDataDTO>> GetMediaDataListAsync(Guid associatedRecordId)
        {
            var mediaList = await _context.MediaData
                .Where(m => m.AssociatedRecordId == associatedRecordId)
                .ToListAsync();

            return mediaList.Select(m => new MediaDataDTO
            {
                Id = m.Id,
                Name = m.Name,
                Extension = m.Extension,
                Content = m.Content,
                AssociatedRecordId = m.AssociatedRecordId,
                ObjectTypeId = m.ObjectTypeId,
                IsPrime = m.IsPrime
            }).ToList();
        }

        public async Task AddMediaAsync(Guid associatedRecordId, List<IFormFile> imageFiles, string objectTypeName)
        {
            var objectType = _context.ObjectTypes.FirstOrDefault(t => t.Name == objectTypeName);

            if (imageFiles != null)
            {
                foreach (var file in imageFiles)
                {
                    using var stream = new MemoryStream();
                    await file.CopyToAsync(stream);

                    var media = new MediaDatum
                    {
                        Id = Guid.NewGuid(),
                        Name = file.FileName,
                        Extension = Path.GetExtension(file.FileName),
                        Content = stream.ToArray(),
                        AssociatedRecordId = associatedRecordId,
                        ObjectTypeId = objectType?.Id ?? 1,
                        IsPrime = false
                    };
                    await _context.MediaData.AddAsync(media);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteMediaAsync(List<Guid> mediaIds)
        {
            foreach (var id in mediaIds)
            {
                var media = await _context.MediaData.FindAsync(id);
                if (media != null)
                {
                    _context.MediaData.Remove(media);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
