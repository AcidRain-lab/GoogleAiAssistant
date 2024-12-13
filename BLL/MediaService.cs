using BLL.DTO;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Http;

namespace BLL.Services
{
    public class MediaService
    {
        private readonly CrmContext _context;

        public MediaService(CrmContext context)
        {
            _context = context;
        }

        public bool HasMedia(Guid objectId)
        {
            return _context.MediaData.Any(m => m.AssociatedRecordId == objectId);
        }


        public bool HasAvatar(Guid objectId)
        {
            return _context.Avatars.Any(m => m.AssociatedRecordId == objectId);
        }


        public async Task<bool> SetAvatarAsync(AvatarDTO model, Guid associatedRecordId, string objectTypeName)
        {
    
            var existingMediaData = await _context.Avatars.FirstOrDefaultAsync(m => m.AssociatedRecordId == associatedRecordId);
            var ObjectType = _context.ObjectTypes.FirstOrDefault(t => t.Name == objectTypeName);

            if (existingMediaData != null && model.IsDeletedImg == true)
            {
                _context.Avatars.Remove(existingMediaData);
                await _context.SaveChangesAsync();
            }
            else if (!string.IsNullOrEmpty(model.Base64Image) && !string.IsNullOrEmpty(model.ImgName))
            {
                byte[] imageData = Convert.FromBase64String(model.Base64Image);

                imageData = await ResizeAndCompressImageAsync(imageData);

                if (existingMediaData != null)
                {
                    existingMediaData.Name = model.ImgName;
                    existingMediaData.Extension = Path.GetExtension(model.ImgName);
                    existingMediaData.Content = imageData;
                    _context.Avatars.Update(existingMediaData);
                }
                else
                {
                    var mediaData = new Avatar
                    {
                        Name = model.ImgName,
                        Extension = Path.GetExtension(model.ImgName),
                        Content = imageData,
                        AssociatedRecordId = associatedRecordId,
                        ObjectTypeId = ObjectType?.Id ?? 1 // Предполагаем, что ID типа объекта для изображений равен 1
                    };

                    _context.Avatars.Add(mediaData);
                }
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private async Task<byte[]> ResizeAndCompressImageAsync(byte[] imageData)
        {
            using var image = Image.Load(imageData);
            image.Mutate(x => x.Resize(100, 100));

            using var outputStream = new MemoryStream();
            await image.SaveAsJpegAsync(outputStream, new JpegEncoder { Quality = 75 }); // Сжатие до качества 75%
            return outputStream.ToArray();
        }

        public async Task<List<MediaDatum>> GetMediaDataListAsync(Guid associatedRecordId)
        {
            return await _context.MediaData
                .Where(m => m.AssociatedRecordId == associatedRecordId)
                .ToListAsync();
        }


        public async Task SetGalleryAsync(Guid associatedRecordId, List<IFormFile> imageFiles, List<Guid> removedImageIds, string objectTypeName)
        {
            // Удаление изображений
            if (removedImageIds != null)
            {
                foreach (var imageId in removedImageIds)
                {
                    var mediaData = await _context.MediaData.FirstOrDefaultAsync(m => m.Id == imageId);
                    if (mediaData != null)
                    {
                        _context.MediaData.Remove(mediaData);
                    }
                }
                await _context.SaveChangesAsync();
            }

            // Сохранение новых изображений
            if (imageFiles != null && imageFiles.Count > 0)
            {
                foreach (var file in imageFiles)
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        var imageData = stream.ToArray();

                        var mediaData = new MediaDatum
                        {
                            Id = Guid.NewGuid(),
                            Name = file.FileName,
                            Extension = Path.GetExtension(file.FileName),
                            Content = imageData,
                            AssociatedRecordId = associatedRecordId,
                            IsPrime = false,
                            ObjectTypeId = 1 // Здесь предполагается, что у вас есть соответствующее определение типа объекта
                        };

                        await _context.MediaData.AddAsync(mediaData);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        //--------------
        public static string GetAvatarBase64(Guid associatedRecordId, CrmContext context)
        {
            var mediaData = context.Avatars.FirstOrDefault(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData != null ? Convert.ToBase64String(mediaData.Content) : string.Empty;
        }


        public  byte[]? GetAvatarBytes(Guid associatedRecordId)
        {
            var mediaData = _context.Avatars.FirstOrDefault(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData?.Content;
        }

        public  async Task<bool> HasAvatarAsync(Guid associatedRecordId)
        {
            var exists = await _context.Avatars.AnyAsync(m => m.AssociatedRecordId == associatedRecordId);
            return exists;
        }


        public  async Task<string> GetAvatarBase64Async(Guid associatedRecordId)
        {
            var mediaData = await _context.Avatars.FirstOrDefaultAsync(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData != null ? Convert.ToBase64String(mediaData.Content) : string.Empty;
        }

        public async Task<byte[]>? GetAvatarBytesAsync(Guid associatedRecordId)
        {
            var mediaData = await _context.Avatars.FirstOrDefaultAsync(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData?.Content;
        }

    }
}
