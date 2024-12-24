using DAL.Models;
using MediaLib.DTO;
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
                Base64Image = d.Content != null ? Convert.ToBase64String(d.Content) : null
            }).ToList();
        }

        public async Task AddDocumentsAsync(List<DocumentsDTO> documents)
        {
            foreach (var document in documents)
            {
                var newDocument = new DocumentsDatum
                {
                    Id = Guid.NewGuid(),
                    Name = document.Name,
                    Extension = document.Extension,
                    Content = document.Content,
                    AssociatedRecordId = document.AssociatedRecordId,
                    ObjectTypeId = document.ObjectTypeId
                };
                await _context.DocumentsData.AddAsync(newDocument);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateDocumentsAsync(List<DocumentsDTO> documents)
        {
            foreach (var document in documents)
            {
                var existingDocument = await _context.DocumentsData.FindAsync(document.Id);
                if (existingDocument != null)
                {
                    existingDocument.Name = document.Name;
                    existingDocument.Extension = document.Extension;
                    existingDocument.Content = document.Content;
                }
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

        public async Task<bool> RemoveDocumentAsync(Guid documentId)
        {
            var document = await _context.DocumentsData.FindAsync(documentId);
            if (document == null) return false;

            _context.DocumentsData.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
