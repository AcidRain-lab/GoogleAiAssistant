using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class MessageService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public MessageService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageDTO>> GetByUserIdAsync(Guid userId)
        {
            var messages = await _context.Messages
                .Where(m => m.UserId == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }

        public async Task<MessageDTO?> GetByIdAsync(Guid id)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            return message == null ? null : _mapper.Map<MessageDTO>(message);
        }

        public async Task CreateAsync(MessageDTO messageDto)
        {
            var message = _mapper.Map<Message>(messageDto);
            message.Id = Guid.NewGuid();
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MessageDTO messageDto)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageDto.Id);
            if (message == null)
                throw new KeyNotFoundException($"Message with ID {messageDto.Id} not found.");

            _mapper.Map(messageDto, message);
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
                throw new KeyNotFoundException($"Message with ID {id} not found.");

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}
