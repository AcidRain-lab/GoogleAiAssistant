using DAL.Models;
using MediaLib.DTO;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MediaLib.Services
{
    public class AvatarService
    {
        private readonly BankContext _context;

        public AvatarService(BankContext context)
        {
            _context = context;
        }

        public async Task<bool> HasAvatarAsync(Guid associatedRecordId)
        {
            return await _context.Avatars.AnyAsync(m => m.AssociatedRecordId == associatedRecordId);
        }

        public async Task<string?> GetAvatarBase64Async(Guid associatedRecordId)
        {
            var avatar = await _context.Avatars.FirstOrDefaultAsync(m => m.AssociatedRecordId == associatedRecordId);
            return avatar != null ? Convert.ToBase64String(avatar.Content) : null;
        }

        public async Task<byte[]?> GetAvatarBytesAsync(Guid associatedRecordId)
        {
            var avatar = await _context.Avatars.FirstOrDefaultAsync(m => m.AssociatedRecordId == associatedRecordId);
            return avatar?.Content;
        }

        public async Task<bool> SetAvatarAsync(AvatarDTO model, Guid associatedRecordId, string objectTypeName)
        {
            var existingAvatar = await _context.Avatars.FirstOrDefaultAsync(m => m.AssociatedRecordId == associatedRecordId);
            var objectType = _context.ObjectTypes.FirstOrDefault(t => t.Name == objectTypeName);

            if (existingAvatar != null && model.IsDeletedImg == true)
            {
                _context.Avatars.Remove(existingAvatar);
                await _context.SaveChangesAsync();
                return true;
            }

            if (!string.IsNullOrEmpty(model.Base64Image) && !string.IsNullOrEmpty(model.ImgName))
            {
                byte[] imageData = Convert.FromBase64String(model.Base64Image);
                imageData = await ResizeAndCompressImageAsync(imageData);

                if (existingAvatar != null)
                {
                    existingAvatar.Name = model.ImgName;
                    existingAvatar.Extension = Path.GetExtension(model.ImgName);
                    existingAvatar.Content = imageData;
                    _context.Avatars.Update(existingAvatar);
                }
                else
                {
                    var avatar = new Avatar
                    {
                        Name = model.ImgName,
                        Extension = Path.GetExtension(model.ImgName),
                        Content = imageData,
                        AssociatedRecordId = associatedRecordId,
                        ObjectTypeId = objectType?.Id ?? 1
                    };
                    _context.Avatars.Add(avatar);
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
            await image.SaveAsJpegAsync(outputStream, new JpegEncoder { Quality = 75 });
            return outputStream.ToArray();
        }
    }
}
