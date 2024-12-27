using DAL.Models;
using MediaLib.DTO;
using MediaLib.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task ManageMediaAsync(
            Guid recordId,
            List<IFormFile>? newFiles,
            List<Guid>? mediaToDelete,
            Guid? primaryMediaId,
            ObjectType objectType)
        {
            if (mediaToDelete != null && mediaToDelete.Any())
            {
                foreach (var mediaId in mediaToDelete)
                {
                    await RemoveMediaAsync(mediaId);
                }
            }

            if (newFiles != null && newFiles.Any())
            {
                var newMediaFiles = await FileHelper.CreateDTOListFromUploadedFilesAsync<MediaDataDTO>(newFiles);
                foreach (var media in newMediaFiles)
                {
                    media.AssociatedRecordId = recordId;
                    media.ObjectTypeId = (int)objectType;
                }
                await AddMediaAsync(newMediaFiles);
            }

            var remainingMedia = await GetMediaDataListAsync(recordId);
            if (primaryMediaId.HasValue)
            {
                var primaryMediaExists = remainingMedia.Any(m => m.Id == primaryMediaId.Value);
                if (primaryMediaExists)
                {
                    await SetPrimaryMediaAsync(recordId, primaryMediaId.Value);
                }
                else if (remainingMedia.Any())
                {
                    var firstMedia = remainingMedia.First();
                    await SetPrimaryMediaAsync(recordId, firstMedia.Id);
                }
            }
            else if (remainingMedia.Any())
            {
                var firstMedia = remainingMedia.First();
                await SetPrimaryMediaAsync(recordId, firstMedia.Id);
            }
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
                Base64Image = m.Content != null ? FileHelper.ToBase64(m.Content) : null
            }).ToList();
        }

        public async Task<MediaDataDTO?> GetPrimaryMediaAsync(Guid recordId)
        {
            var primaryMedia = await _context.MediaData
                .Where(m => m.AssociatedRecordId == recordId && m.IsPrime)
                .FirstOrDefaultAsync();

            if (primaryMedia == null) return null;

            return new MediaDataDTO
            {
                Id = primaryMedia.Id,
                Name = primaryMedia.Name,
                Extension = primaryMedia.Extension,
                Content = primaryMedia.Content,
                AssociatedRecordId = primaryMedia.AssociatedRecordId,
                ObjectTypeId = primaryMedia.ObjectTypeId,
                IsPrime = primaryMedia.IsPrime,
                Base64Image = primaryMedia.Content != null ? Convert.ToBase64String(primaryMedia.Content) : null
            };
        }

        public async Task AddMediaAsync(List<MediaDataDTO> mediaFiles)
        {
            foreach (var media in mediaFiles)
            {
                if (media.UploadedFile != null)
                {
                    media.Content = await FileHelper.ConvertToByteArrayAsync(media.UploadedFile);
                    media.Extension = Path.GetExtension(media.UploadedFile.FileName);
                    media.Name = FileHelper.GenerateUniqueFileName(media.UploadedFile.FileName);
                }

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

        public async Task<bool> RemoveMediaAsync(Guid mediaId)
        {
            var media = await _context.MediaData.FindAsync(mediaId);
            if (media == null) return false;

            var associatedRecordId = media.AssociatedRecordId;
            _context.MediaData.Remove(media);
            await _context.SaveChangesAsync();

            var remainingMedia = await _context.MediaData
                .Where(m => m.AssociatedRecordId == associatedRecordId)
                .ToListAsync();

            if (remainingMedia.Any())
            {
                var currentPrime = remainingMedia.FirstOrDefault(m => m.IsPrime);
                if (currentPrime == null)
                {
                    var firstMedia = remainingMedia.First();
                    firstMedia.IsPrime = true;
                    _context.MediaData.Update(firstMedia);
                    await _context.SaveChangesAsync();
                }
            }

            return true;
        }

        public async Task SetPrimaryMediaAsync(Guid recordId, Guid primaryMediaId)
        {
            var mediaList = await _context.MediaData
                .Where(m => m.AssociatedRecordId == recordId)
                .ToListAsync();

            foreach (var media in mediaList)
            {
                media.IsPrime = false;
            }

            var primaryMedia = mediaList.FirstOrDefault(m => m.Id == primaryMediaId);
            if (primaryMedia != null)
            {
                primaryMedia.IsPrime = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveMediaByRecordIdAsync(Guid recordId)
        {
            var mediaFiles = await _context.MediaData
                .Where(m => m.AssociatedRecordId == recordId)
                .ToListAsync();

            if (!mediaFiles.Any())
                return false;

            _context.MediaData.RemoveRange(mediaFiles);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
