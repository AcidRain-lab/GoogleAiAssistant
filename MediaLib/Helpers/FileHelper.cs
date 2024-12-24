using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace MediaLib.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// Преобразует массив байтов в строку Base64.
        /// </summary>
        public static string ToBase64(byte[] content)
        {
            return Convert.ToBase64String(content);
        }

        /// <summary>
        /// Преобразует строку Base64 в массив байтов.
        /// </summary>
        public static byte[] FromBase64(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// Преобразует файл в массив байтов.
        /// </summary>
        public static byte[] ConvertToByteArray(Stream fileStream)
        {
            using var memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Проверяет расширение файла.
        /// </summary>
        public static bool IsAllowedExtension(string fileName, string[] allowedExtensions)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return Array.Exists(allowedExtensions, ext => ext == extension);
        }

        /// <summary>
        /// Сжимает и изменяет размер изображения.
        /// </summary>
        public static byte[] ResizeAndCompressImage(byte[] imageData, int width, int height, int quality = 75)
        {
            using var image = Image.Load(imageData);
            image.Mutate(x => x.Resize(width, height));

            using var outputStream = new MemoryStream();
            image.SaveAsJpeg(outputStream, new JpegEncoder { Quality = quality });
            return outputStream.ToArray();
        }

        /// <summary>
        /// Генерирует уникальное имя файла.
        /// </summary>
        public static string GenerateUniqueFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            return $"{Guid.NewGuid()}{extension}";
        }
    }
}
