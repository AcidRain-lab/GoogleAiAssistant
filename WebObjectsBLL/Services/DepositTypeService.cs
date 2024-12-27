using AutoMapper;
using DAL.Models;
using MediaLib.DTO;
using MediaLib.Helpers;
using MediaLib.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;
using System.Collections.Generic;

namespace WebObjectsBLL.Services
{
    public class DepositTypeService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;
        private readonly DocumentService _documentService;

        public DepositTypeService(
            BankContext context,
            IMapper mapper,
            DocumentService documentService)
        {
            _context = context;
            _mapper = mapper;
            _documentService = documentService;
        }

        public async Task<IEnumerable<DepositTypeDTO>> GetAllAsync()
        {
            var depositTypes = await _context.DepositTypes
                .Include(dt => dt.DepositTerms)
                .ToListAsync();
            return _mapper.Map<IEnumerable<DepositTypeDTO>>(depositTypes);
        }

        public async Task<DepositTypeDTO> GetByIdWithDetailsAsync(Guid id)
        {
            var depositType = await _context.DepositTypes
                .Include(dt => dt.DepositTerms)
                .FirstOrDefaultAsync(dt => dt.Id == id);

            if (depositType == null)
                throw new KeyNotFoundException("Deposit type not found");

            var dto = _mapper.Map<DepositTypeDTO>(depositType);
            dto.Documents = await _documentService.GetDocumentsListAsync(id);

            return dto;
        }

        public async Task AddAsync(
            DepositTypeDTO depositTypeDto,
            List<DocumentsDTO>? documents)
        {
            var depositType = _mapper.Map<DepositType>(depositTypeDto);
            depositType.Id = Guid.NewGuid();
            _context.DepositTypes.Add(depositType);
            await _context.SaveChangesAsync();

            if (documents != null)
            {
                await _documentService.ManageDocumentsAsync(
                    depositType.Id,
                    null,
                    null,
                    null,
                    MediaLib.ObjectType.DepositType);
            }
        }

        public async Task UpdateAsync(
            DepositTypeDTO depositTypeDto,
            List<IFormFile>? newDocumentFiles,
            List<Guid>? documentsToDelete,
            Guid? primaryDocumentId)
        {
            var depositType = await _context.DepositTypes.FirstOrDefaultAsync(dt => dt.Id == depositTypeDto.Id);
            if (depositType == null)
                throw new KeyNotFoundException("Deposit type not found");

            _mapper.Map(depositTypeDto, depositType);
            await _context.SaveChangesAsync();

            await _documentService.ManageDocumentsAsync(
                depositType.Id,
                newDocumentFiles,
                documentsToDelete,
                primaryDocumentId,
                MediaLib.ObjectType.DepositType);
        }

        public async Task DeleteAsync(Guid id)
        {
            var depositType = await _context.DepositTypes.FirstOrDefaultAsync(dt => dt.Id == id);
            if (depositType == null)
                throw new KeyNotFoundException("Deposit type not found");

            _context.DepositTypes.Remove(depositType);
            await _context.SaveChangesAsync();

            await _documentService.RemoveDocumentsByRecordIdAsync(id);
        }

        public async Task DeleteDocumentAsync(Guid documentId)
        {
            var success = await _documentService.RemoveDocumentAsync(documentId);

            if (!success)
            {
                throw new KeyNotFoundException($"Document with ID {documentId} not found.");
            }
        }

        public async Task<List<DepositTypeDTO>> GetDepositTypesAsync()
        {
            var depositTypes = await _context.DepositTypes
                         .Include(dt => dt.DepositTerms)
                         .ToListAsync();

            foreach (var depositType in depositTypes)
            {
                
            }

            return _mapper.Map<List<DepositTypeDTO>>(depositTypes);
        }
    }
}