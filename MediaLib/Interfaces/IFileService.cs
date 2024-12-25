using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaLib.Interfaces
{
    public interface IFileService<TDto>
        where TDto : class
    {
        /// <summary>
        /// Управление файлами для записи.
        /// </summary>
        /// <param name="recordId">Идентификатор записи, к которой относятся файлы.</param>
        /// <param name="newFiles">Новые файлы для добавления.</param>
        /// <param name="filesToDelete">Список идентификаторов файлов для удаления.</param>
        /// <param name="primaryFileId">Идентификатор файла, который будет установлен как Primary.</param>
        /// <param name="objectType">Тип объекта, связанного с файлами.</param>
        Task ManageFilesAsync(
            Guid recordId,
            List<IFormFile>? newFiles,
            List<Guid>? filesToDelete,
            Guid? primaryFileId,
            ObjectType? objectType = null);

        /// <summary>
        /// Получение списка файлов для указанной записи.
        /// </summary>
        /// <param name="recordId">Идентификатор записи.</param>
        /// <returns>Список DTO файлов.</returns>
        Task<List<TDto>> GetFileListAsync(Guid recordId);

        /// <summary>
        /// Добавление новых файлов.
        /// </summary>
        /// <param name="files">Список файлов для добавления.</param>
        Task AddFilesAsync(List<TDto> files);

        /// <summary>
        /// Удаление файла по идентификатору.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>True, если файл удален; иначе False.</returns>
        Task<bool> RemoveFileAsync(Guid fileId);

        /// <summary>
        /// Удаление всех файлов для указанной записи.
        /// </summary>
        /// <param name="recordId">Идентификатор записи.</param>
        /// <returns>True, если файлы удалены; иначе False.</returns>
        Task<bool> RemoveFilesByRecordIdAsync(Guid recordId);

        /// <summary>
        /// Установить файл как Primary.
        /// </summary>
        /// <param name="recordId">Идентификатор записи.</param>
        /// <param name="primaryFileId">Идентификатор файла, который будет установлен как Primary.</param>
        Task SetPrimaryFileAsync(Guid recordId, Guid primaryFileId);
    }
}
