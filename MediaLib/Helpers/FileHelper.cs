using MediaLib.DTO;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static async Task<T?> CreateDTOFromUploadedFileAsync<T>(IFormFile file) where T : class, new()
        {
            if (file == null) return null;

            var dto = new T();

            if (dto is AvatarDTO avatar)
            {
                avatar.Content = await ConvertToByteArrayAsync(file);
                avatar.Name = GenerateUniqueFileName(file.FileName);
                avatar.Extension = Path.GetExtension(file.FileName);
                avatar.UploadedFile = file; // Сохраняем ссылку на файл
                return avatar as T;
            }

            if (dto is MediaDataDTO media)
            {
                media.Content = await ConvertToByteArrayAsync(file);
                media.Name = file.FileName;
                media.Extension = Path.GetExtension(file.FileName);
                media.UploadedFile = file; // Сохраняем ссылку на файл
                return media as T;
            }

            if (dto is DocumentsDTO document)
            {
                document.Content = await ConvertToByteArrayAsync(file);
                document.Name = file.FileName;
                document.Extension = Path.GetExtension(file.FileName);
                document.UploadedFile = file; // Сохраняем ссылку на файл
                return document as T;
            }

            return null;
        }

        public static async Task<List<T>> CreateDTOListFromUploadedFilesAsync<T>(List<IFormFile>? files) where T : class, new()
        {
            if (files == null || !files.Any()) return new List<T>();

            var dtoList = new List<T>();

            foreach (var file in files)
            {
                var dto = await CreateDTOFromUploadedFileAsync<T>(file);
                if (dto != null)
                {
                    dtoList.Add(dto);
                }
            }

            return dtoList;
        }

    }
}
