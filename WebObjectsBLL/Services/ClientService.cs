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
            _mapper = mapper;
            _avatarService = avatarService;
        }

        public async Task<IEnumerable<ClientDTO>> GetAllAsync()
        {
            var clients = await _context.Clients.ToListAsync();

            var clientDtos = clients.Select(client => new ClientDTO
            {
                Id = client.Id,
                Name = $"{client.FirstName} {client.LastName}",
                Email = client.Email,
                Phone = client.Phone,
                IsActive = client.IsActive ?? false
            });

            return clientDtos;
        }

        public async Task<ClientDetailDTO?> GetDetailByIdAsync(Guid id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
                return null; // Return null if the client is not found

            return _mapper.Map<ClientDetailDTO>(client);
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

            // Update data
            client.FirstName = clientDetailDto.FirstName;
            client.LastName = clientDetailDto.LastName;
            client.Email = clientDetailDto.Email;
            client.Phone = clientDetailDto.Phone;
            client.BirthDate = clientDetailDto.BirthDate.HasValue ? DateOnly.FromDateTime(clientDetailDto.BirthDate.Value) : null;
            client.PassportData = clientDetailDto.PassportData;
            client.TaxId = clientDetailDto.TaxId;
            client.IsActive = clientDetailDto.IsActive;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }
        public async Task AvatarUpdateAsync(Guid clientId, AvatarDTO? avatar)
        {
            // Получаем клиента
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            if (client == null)
            {
                throw new KeyNotFoundException($"Client with ID {clientId} not found.");
            }

            // Обновляем аватар
            if (avatar != null && avatar.Content != null)
            {
                avatar.AssociatedRecordId = client.Id;
                avatar.ObjectTypeId = (int)MediaLib.ObjectType.Client;
                await _avatarService.SetAvatarAsync(avatar);
            }
            else
            {
                // Удаляем аватар, если передан null
                await _avatarService.RemoveAvatarAsync(client.Id);
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
