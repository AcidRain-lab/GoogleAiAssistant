using DAL.Models;
using MediaLib.DTO;
using MediaLib.Helpers;
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

        /// <summary>
        /// Получение списка медиа-данных для записи.
        /// </summary>
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

        /// <summary>
        /// Добавление новых медиа-файлов.
        /// </summary>
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

        /// <summary>
        /// Обновление существующих медиа-файлов.
        /// </summary>
        public async Task UpdateMediaAsync(List<MediaDataDTO> mediaFiles)
        {
            foreach (var media in mediaFiles)
            {
                // Попытка найти существующий медиа-файл по ID
                var existingMedia = await _context.MediaData.FindAsync(media.Id);

                if (existingMedia != null)
                {
                    // Обновление существующего медиа-файла
                    existingMedia.Name = media.Name;
                    existingMedia.Extension = media.Extension;

                    if (media.UploadedFile != null)
                    {
                        existingMedia.Content = await FileHelper.ConvertToByteArrayAsync(media.UploadedFile);
                    }

                    existingMedia.IsPrime = media.IsPrime;
                }
                else
                {
                    // Создание нового медиа-файла, если он не существует
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
                        IsPrime = mediaFiles.Count == 1 // Если это единственный файл, делаем его основным
                    };

                    await _context.MediaData.AddAsync(newMedia);
                }
            }

            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Удаление медиа-файлов по идентификатору записи.
        /// </summary>
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

        /// <summary>
        /// Удаление конкретного медиа-файла по идентификатору.
        /// </summary>
        public async Task<bool> RemoveMediaAsync(Guid mediaId)
        {
            var media = await _context.MediaData.FindAsync(mediaId);
            if (media == null) return false;

            _context.MediaData.Remove(media);
            await _context.SaveChangesAsync();
            return true;
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
    }
}
