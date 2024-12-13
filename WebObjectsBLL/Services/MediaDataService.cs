using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class MediaDataService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public MediaDataService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MediaDataDTO>> GetAllAsync()
        {
            var mediaData = await _context.MediaData.ToListAsync();
            return _mapper.Map<IEnumerable<MediaDataDTO>>(mediaData);
        }

        public async Task<MediaDataDTO> GetByIdAsync(Guid id)
        {
            var mediaData = await _context.MediaData.FindAsync(id);
            return _mapper.Map<MediaDataDTO>(mediaData);
        }

        public async Task CreateAsync(MediaDataDTO dto)
        {
            var mediaData = _mapper.Map<MediaDatum>(dto);
            _context.MediaData.Add(mediaData);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MediaDataDTO dto)
        {
            var mediaData = _mapper.Map<MediaDatum>(dto);
            _context.MediaData.Update(mediaData);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var mediaData = await _context.MediaData.FindAsync(id);
            if (mediaData != null)
            {
                _context.MediaData.Remove(mediaData);
                await _context.SaveChangesAsync();
            }
        }
    }
}
