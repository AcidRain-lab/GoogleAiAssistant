using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLL.DTO;


namespace BLL
{
    public static class MediaHelpers
    {

        public static string GetAvatarBase64(Guid associatedRecordId, CrmContext context)
        {
            var mediaData = context.Avatars.FirstOrDefault(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData != null ? Convert.ToBase64String(mediaData.Content) : string.Empty;
        }


        public static byte[]? GetAvatarBytes(Guid associatedRecordId, CrmContext context)
        {
            var mediaData = context.Avatars.FirstOrDefault(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData?.Content;
        }

        public static async Task<bool> HasAvatarAsync(Guid associatedRecordId, CrmContext context)
        {
            var exists = await context.Avatars.AnyAsync(m => m.AssociatedRecordId == associatedRecordId);
            return exists;
        }


        public static async Task<string> GetAvatarBase64Async(Guid associatedRecordId, CrmContext context)
        {
            var mediaData = await context.Avatars.FirstOrDefaultAsync(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData != null ? Convert.ToBase64String(mediaData.Content) : string.Empty;
        }

        public static async Task<byte[]>? GetAvatarBytesAsync(Guid associatedRecordId, CrmContext context)
        {
            var mediaData = await context.Avatars.FirstOrDefaultAsync(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData?.Content;
        }

    }
}
