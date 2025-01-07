using AutoMapper;
using DAL.Models;
using MediaLib.DTO;
using MediaLib.Services;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class ClientService
    {
        private readonly BankContext _context;
        private readonly AvatarService _avatarService;
        private readonly IMapper _mapper;

        public ClientService(BankContext context, AvatarService avatarService, IMapper mapper)
        {
            _context = context;
            _avatarService = avatarService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDTO>> GetAllAsync()
        {
            var clients = await _context.Clients.ToListAsync();
            return clients.Select(client => _mapper.Map<ClientDTO>(client));
        }

        public async Task<ClientDetailDTO?> GetDetailByIdAsync(Guid id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
                return null;

            var clientDetail = _mapper.Map<ClientDetailDTO>(client);
            clientDetail.Avatar = await _avatarService.GetAvatarAsync(id);

            return clientDetail;
        }

        public async Task<ClientDetailDTO?> GetClientByUserIdAsync(Guid userId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client == null)
                return null;

            var clientDetail = _mapper.Map<ClientDetailDTO>(client);
            clientDetail.Avatar = await _avatarService.GetAvatarAsync(client.Id);

            return clientDetail;
        }

        public async Task CreateAsync(ClientDetailDTO clientDetailDto)
        {
            var client = _mapper.Map<Client>(clientDetailDto);
            client.Id = Guid.NewGuid();
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ClientDetailDTO clientDetailDto)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == clientDetailDto.Id);
            if (client == null)
                throw new KeyNotFoundException($"Client with ID {clientDetailDto.Id} not found.");

            _mapper.Map(clientDetailDto, client);
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task AvatarUpdateAsync(Guid clientId, AvatarDTO? avatar)
        {
            if (avatar != null && avatar.Content != null)
            {
                avatar.AssociatedRecordId = clientId;
                avatar.ObjectTypeId = (int)MediaLib.ObjectType.Client;
                await _avatarService.SetAvatarAsync(avatar);
            }
            else
            {
                await _avatarService.RemoveAvatarAsync(clientId);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
                throw new KeyNotFoundException($"Client with ID {id} not found.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }
}
