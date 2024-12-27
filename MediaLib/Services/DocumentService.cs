using DAL.Models;
using MediaLib.DTO;
using MediaLib.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MediaLib.Services
{
    public class DocumentService
    {
        private readonly BankContext _context;

        public DocumentService(BankContext context)
        {
            _context = context;
        }

        public async Task ManageDocumentsAsync(
            Guid recordId,
            List<IFormFile>? newFiles,
            List<Guid>? documentsToDelete,
            Guid? primaryDocumentId,
            ObjectType objectType,
            Guid ownerId)
        {
            if (documentsToDelete != null && documentsToDelete.Any())
            {
                foreach (var documentId in documentsToDelete)
                {
                    await RemoveDocumentAsync(documentId);
                }
            }

            if (newFiles != null && newFiles.Any())
            {
                var newDocuments = await FileHelper.CreateDTOListFromUploadedFilesAsync<DocumentsDTO>(newFiles);
                foreach (var document in newDocuments)
                {
                    document.AssociatedRecordId = recordId;
                    document.ObjectTypeId = (int)objectType;
                    document.OwnerId = ownerId;
                }
                await AddDocumentsAsync(newDocuments);
            }

            var remainingDocuments = await GetDocumentsListAsync(recordId);
            if (primaryDocumentId.HasValue)
            {
                var primaryDocumentExists = remainingDocuments.Any(d => d.Id == primaryDocumentId.Value);
                if (primaryDocumentExists)
                {
                    await SetPrimaryDocumentAsync(recordId, primaryDocumentId.Value);
                }
                else if (remainingDocuments.Any())
                {
                    var firstDocument = remainingDocuments.First();
                    await SetPrimaryDocumentAsync(recordId, firstDocument.Id);
                }
            }
        }

        public async Task<List<DocumentsDTO>> GetDocumentsListAsync(Guid recordId)
        {
            var documents = await _context.DocumentsData
                .Where(d => d.AssociatedRecordId == recordId)
                .ToListAsync();

            return documents.Select(d => new DocumentsDTO
            {
                Id = d.Id,
                Name = d.Name,
                Extension = d.Extension,
                Content = d.Content,
                AssociatedRecordId = d.AssociatedRecordId,
                ObjectTypeId = d.ObjectTypeId,
                OwnerId = d.OwnerId,
                IsPrime = d.IsPrime,
                Base64Image = d.Content != null ? Convert.ToBase64String(d.Content) : null
            }).ToList();
        }

        public async Task AddDocumentsAsync(List<DocumentsDTO> documents)
        {
            foreach (var document in documents)
            {
                if (document.UploadedFile != null)
                {
                    document.Content = await FileHelper.ConvertToByteArrayAsync(document.UploadedFile);
                    document.Extension = Path.GetExtension(document.UploadedFile.FileName);
                    document.Name = FileHelper.GenerateUniqueFileName(document.UploadedFile.FileName);
                }

                var newDocument = new DocumentsDatum
                {
                    Id = Guid.NewGuid(),
                    Name = document.Name,
                    Extension = document.Extension,
                    Content = document.Content,
                    AssociatedRecordId = document.AssociatedRecordId,
                    ObjectTypeId = document.ObjectTypeId,
                    OwnerId = document.OwnerId,
                    IsPrime = document.IsPrime
                };
                await _context.DocumentsData.AddAsync(newDocument);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveDocumentAsync(Guid documentId)
        {
            var document = await _context.DocumentsData.FindAsync(documentId);
            if (document == null) return false;

            _context.DocumentsData.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SetPrimaryDocumentAsync(Guid recordId, Guid primaryDocumentId)
        {
            var documentsList = await _context.DocumentsData
                .Where(d => d.AssociatedRecordId == recordId)
                .ToListAsync();

            foreach (var document in documentsList)
            {
                document.IsPrime = false;
            }

            var primaryDocument = documentsList.FirstOrDefault(d => d.Id == primaryDocumentId);
            if (primaryDocument != null)
            {
                primaryDocument.IsPrime = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveDocumentsByRecordIdAsync(Guid recordId)
        {
            var documents = await _context.DocumentsData
                .Where(d => d.AssociatedRecordId == recordId)
                .ToListAsync();

            if (!documents.Any()) return false;

            _context.DocumentsData.RemoveRange(documents);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DocumentsDTO?> GetDocumentByIdAsync(Guid id)
        {
            var document = await _context.DocumentsData.FirstOrDefaultAsync(d => d.Id == id);
            if (document == null) return null;

            return new DocumentsDTO
            {
                Id = document.Id,
                Name = document.Name,
                Extension = document.Extension,
                Content = document.Content,
                AssociatedRecordId = document.AssociatedRecordId,
                ObjectTypeId = document.ObjectTypeId,
                OwnerId = document.OwnerId,
                IsPrime = document.IsPrime
            };
        }
    }
}
