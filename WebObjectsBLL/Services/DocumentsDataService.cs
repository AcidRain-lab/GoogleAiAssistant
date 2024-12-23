using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class DocumentsDataService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public DocumentsDataService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DocumentsDataDTO>> GetAllAsync()
        {
            var documentsData = await _context.DocumentsData.ToListAsync();
            return _mapper.Map<IEnumerable<DocumentsDataDTO>>(documentsData);
        }

        public async Task<DocumentsDataDTO> GetByIdAsync(Guid id)
        {
            var documentData = await _context.DocumentsData.FindAsync(id);
            return _mapper.Map<DocumentsDataDTO>(documentData);
        }

        public async Task CreateAsync(DocumentsDataDTO dto)
        {
            var documentData = _mapper.Map<DocumentsDatum>(dto);
            _context.DocumentsData.Add(documentData);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DocumentsDataDTO dto)
        {
            var documentData = _mapper.Map<DocumentsDatum>(dto);
            _context.DocumentsData.Update(documentData);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var documentData = await _context.DocumentsData.FindAsync(id);
            if (documentData != null)
            {
                _context.DocumentsData.Remove(documentData);
                await _context.SaveChangesAsync();
            }
        }
    }
}
