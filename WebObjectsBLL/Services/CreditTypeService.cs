using AutoMapper;
using DAL.Models;
using MediaLib.DTO;
using MediaLib.Helpers;
using MediaLib.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class CreditTypeService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;
        private readonly DocumentService _documentService;

        public CreditTypeService(
            BankContext context,
            IMapper mapper,
            DocumentService documentService)
        {
            _context = context;
            _mapper = mapper;
            _documentService = documentService;
        }

        /* public async Task<IEnumerable<CreditTypeDTO>> GetAllAsync()
         {
             var creditTypes = await _context.Credits.ToListAsync();
             return _mapper.Map<IEnumerable<CreditTypeDTO>>(creditTypes);
         }*/
        public async Task<IEnumerable<CreditTypeDTO>> GetAllAsync()
        {
            var creditTypes = await _context.CreditTypes
                .Include(ct => ct.Credits) // Загружаем связанные кредиты, если нужно
                .ToListAsync();

            return _mapper.Map<IEnumerable<CreditTypeDTO>>(creditTypes);
        }


        public async Task<CreditTypeDTO> GetByIdWithDetailsAsync(Guid id)
        {
            var creditType = await _context.CreditTypes.FirstOrDefaultAsync(ct => ct.Id == id);
            if (creditType == null)
                throw new KeyNotFoundException("Credit type not found");

            var dto = _mapper.Map<CreditTypeDTO>(creditType);
            dto.Documents = await _documentService.GetDocumentsListAsync(id);

            return dto;
        }

        public async Task AddAsync(
             CreditTypeDTO creditTypeDto,
             List<DocumentsDTO>? documents,
             Guid ownerId) // Новый параметр
        {
            var creditType = _mapper.Map<CreditType>(creditTypeDto);
            creditType.Id = Guid.NewGuid();
            _context.CreditTypes.Add(creditType);
            await _context.SaveChangesAsync();

            if (documents != null)
            {
                await _documentService.ManageDocumentsAsync(
                    creditType.Id,
                    null,
                    null,
                    null,
                    MediaLib.ObjectType.CreditType,
                    ownerId); // Передаем ownerId
            }
        }

        public async Task UpdateAsync(
            CreditTypeDTO creditTypeDto,
            List<IFormFile>? newDocumentFiles,
            List<Guid>? documentsToDelete,
            Guid? primaryDocumentId,
            Guid ownerId) // Новый параметр
        {
            var creditType = await _context.CreditTypes.FirstOrDefaultAsync(ct => ct.Id == creditTypeDto.Id);
            if (creditType == null)
                throw new KeyNotFoundException("Credit type not found");

            _mapper.Map(creditTypeDto, creditType);
            await _context.SaveChangesAsync();

            await _documentService.ManageDocumentsAsync(
                creditType.Id,
                newDocumentFiles,
                documentsToDelete,
                primaryDocumentId,
                MediaLib.ObjectType.CreditType,
                ownerId); // Передаем ownerId
        }

        public async Task DeleteAsync(Guid id)
        {
            var creditType = await _context.CreditTypes.FirstOrDefaultAsync(ct => ct.Id == id);
            if (creditType == null)
                throw new KeyNotFoundException("Credit type not found");

            _context.CreditTypes.Remove(creditType);
            await _context.SaveChangesAsync();

            await _documentService.RemoveDocumentsByRecordIdAsync(id);
        }
        public async Task DeleteDocumentAsync(Guid documentId)
        {
            // Call the DocumentService to delete the document
            var success = await _documentService.RemoveDocumentAsync(documentId);

            if (!success)
            {
                throw new KeyNotFoundException($"Document with ID {documentId} not found.");
            }
        }
    }
}
