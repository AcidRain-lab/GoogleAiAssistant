using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MediaLib.Helpers
{
    public static class FileHelper
    {
        public static string ToBase64(byte[] content) => Convert.ToBase64String(content);

        public static byte[] FromBase64(string base64) => Convert.FromBase64String(base64);

        public static async Task<byte[]> ConvertToByteArrayAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public static bool IsAllowedExtension(string fileName, string[] allowedExtensions)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return Array.Exists(allowedExtensions, ext => ext == extension);
        }

        public static byte[] ResizeAndCompressImage(byte[] imageData, int width, int height, int quality = 75)
        {
            using var image = Image.Load(imageData);
            image.Mutate(x => x.Resize(width, height));

            using var outputStream = new MemoryStream();
            image.SaveAsJpeg(outputStream, new JpegEncoder { Quality = quality });
            return outputStream.ToArray();
        }

        public static string GenerateUniqueFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            return $"{Guid.NewGuid()}{extension}";
        }
    }
}
