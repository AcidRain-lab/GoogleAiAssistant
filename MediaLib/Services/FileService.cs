using MediaLib.Helpers;
using MediaLib.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MediaLib.Services
{
    public abstract class FileService<TFileEntity, TDto> : IFileService<TDto>
        where TFileEntity : class, IFileEntity, new()
        where TDto : class, IFileEntity, new()
    {
        protected readonly DbContext _context;

        protected FileService(DbContext context)
        {
            _context = context;
        }

        protected abstract DbSet<TFileEntity> GetDbSet();

        public async Task ManageFilesAsync(
            Guid recordId,
            List<IFormFile>? newFiles,
            List<Guid>? filesToDelete,
            Guid? primaryFileId,
            ObjectType? objectType = null)
        {
            // Удаляем указанные файлы
            if (filesToDelete != null && filesToDelete.Any())
            {
                foreach (var fileId in filesToDelete)
                {
                    await RemoveFileAsync(fileId);
                }
            }

            // Добавляем новые файлы
            if (newFiles != null && newFiles.Any())
            {
                var newFileDtos = await FileHelper.CreateDTOListFromUploadedFilesAsync<TDto>(newFiles);
                foreach (var file in newFileDtos)
                {
                    file.AssociatedRecordId = recordId;
                    file.ObjectTypeId = objectType.HasValue ? (int)objectType : 0;
                }
                await AddFilesAsync(newFileDtos);
            }

            // Обновляем PrimaryFile только после добавления и удаления файлов
            var remainingFiles = await GetFileListAsync(recordId);
            if (primaryFileId.HasValue)
            {
                var primaryFileExists = remainingFiles.Any(f => f.Id == primaryFileId.Value);
                if (primaryFileExists)
                {
                    await SetPrimaryFileAsync(recordId, primaryFileId.Value);
                }
                else if (remainingFiles.Any())
                {
                    var firstFile = remainingFiles.First();
                    await SetPrimaryFileAsync(recordId, firstFile.Id);
                }
            }
            else if (remainingFiles.Any())
            {
                var firstFile = remainingFiles.First();
                await SetPrimaryFileAsync(recordId, firstFile.Id);
            }
        }

        public async Task<List<TDto>> GetFileListAsync(Guid recordId)
        {
            var dbSet = GetDbSet();
            var files = await dbSet.Where(f => f.AssociatedRecordId == recordId).ToListAsync();

            return files.Select(f => new TDto
            {
                Id = f.Id,
                Name = f.Name,
                Extension = f.Extension,
                Content = f.Content,
                AssociatedRecordId = f.AssociatedRecordId,
                ObjectTypeId = f.ObjectTypeId,
                IsPrime = f.IsPrime,
                Base64Image = f.Content != null ? FileHelper.ToBase64(f.Content) : null
            }).ToList();
        }

        public async Task AddFilesAsync(List<TDto> files)
        {
            var dbSet = GetDbSet();
            foreach (var file in files)
            {
                if (file.UploadedFile != null)
                {
                    file.Content = await FileHelper.ConvertToByteArrayAsync(file.UploadedFile);
                    file.Extension = Path.GetExtension(file.UploadedFile.FileName);
                    file.Name = FileHelper.GenerateUniqueFileName(file.UploadedFile.FileName);
                }

                var entity = new TFileEntity
                {
                    Id = Guid.NewGuid(),
                    Name = file.Name,
                    Extension = file.Extension,
                    Content = file.Content,
                    AssociatedRecordId = file.AssociatedRecordId,
                    ObjectTypeId = file.ObjectTypeId,
                    IsPrime = file.IsPrime
                };

                await dbSet.AddAsync(entity);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveFileAsync(Guid fileId)
        {
            var dbSet = GetDbSet();
            var file = await dbSet.FindAsync(fileId);
            if (file == null) return false;

            var associatedRecordId = file.AssociatedRecordId;
            dbSet.Remove(file);
            await _context.SaveChangesAsync();

            var remainingFiles = await dbSet.Where(f => f.AssociatedRecordId == associatedRecordId).ToListAsync();

            if (remainingFiles.Any())
            {
                var currentPrime = remainingFiles.FirstOrDefault(f => f.IsPrime);
                if (currentPrime == null)
                {
                    var firstFile = remainingFiles.First();
                    firstFile.IsPrime = true;
                    dbSet.Update(firstFile);
                    await _context.SaveChangesAsync();
                }
            }

            return true;
        }

        public async Task<bool> RemoveFilesByRecordIdAsync(Guid recordId)
        {
            var dbSet = GetDbSet();
            var files = await dbSet.Where(f => f.AssociatedRecordId == recordId).ToListAsync();

            if (!files.Any())
                return false;

            dbSet.RemoveRange(files);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TDto?> GetPrimaryFileAsync(Guid recordId)
        {
            var dbSet = GetDbSet();
            var primaryFile = await dbSet
                .Where(f => f.AssociatedRecordId == recordId && f.IsPrime)
                .FirstOrDefaultAsync();

            if (primaryFile == null) return null;

            return new TDto
            {
                Id = primaryFile.Id,
                Name = primaryFile.Name,
                Extension = primaryFile.Extension,
                Content = primaryFile.Content,
                AssociatedRecordId = primaryFile.AssociatedRecordId,
                ObjectTypeId = primaryFile.ObjectTypeId,
                IsPrime = primaryFile.IsPrime,
                Base64Image = primaryFile.Content != null ? FileHelper.ToBase64(primaryFile.Content) : null
            };
        }

        public async Task SetPrimaryFileAsync(Guid recordId, Guid primaryFileId)
        {
            var dbSet = GetDbSet();
            var fileList = await dbSet.Where(f => f.AssociatedRecordId == recordId).ToListAsync();

            foreach (var file in fileList)
            {
                file.IsPrime = false;
            }

            var primaryFile = fileList.FirstOrDefault(f => f.Id == primaryFileId);
            if (primaryFile != null)
            {
                primaryFile.IsPrime = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
