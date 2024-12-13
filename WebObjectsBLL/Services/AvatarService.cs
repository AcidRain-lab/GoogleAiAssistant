using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class AvatarService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public AvatarService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AvatarDTO> GetByIdAsync(Guid associatedRecordId)
        {
            var avatar = await _context.Avatars.FindAsync(associatedRecordId);
            return _mapper.Map<AvatarDTO>(avatar);
        }

        public async Task CreateOrUpdateAsync(AvatarDTO dto)
        {
            var avatar = await _context.Avatars.FindAsync(dto.AssociatedRecordId);
            if (avatar == null)
            {
                avatar = _mapper.Map<Avatar>(dto);
                _context.Avatars.Add(avatar);
            }
            else
            {
                _mapper.Map(dto, avatar);
                _context.Avatars.Update(avatar);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid associatedRecordId)
        {
            var avatar = await _context.Avatars.FindAsync(associatedRecordId);
            if (avatar != null)
            {
                _context.Avatars.Remove(avatar);
                await _context.SaveChangesAsync();
            }
        }
    }
}
