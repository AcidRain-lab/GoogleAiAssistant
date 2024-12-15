using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class ClientService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public ClientService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDTO>> GetAllAsync()
        {
            // Получаем всех клиентов
            var clients = await _context.Clients.ToListAsync();

            // Преобразуем клиентов в DTO
            var clientDtos = _mapper.Map<IEnumerable<ClientDTO>>(clients);

            // Для каждого клиента подтягиваем данные из таблиц Individuals или Organizations
            foreach (var clientDto in clientDtos)
            {
                if (clientDto.IsIndividual)
                {
                    var individual = await _context.Individuals.FirstOrDefaultAsync(i => i.ClientId == clientDto.Id);
                    if (individual != null)
                    {
                        clientDto.Name = $"{individual.FirstName} {individual.LastName}";
                    }
                }
                else
                {
                    var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.ClientId == clientDto.Id);
                    if (organization != null)
                    {
                        clientDto.Name = organization.CompanyName;
                    }
                }
            }

            return clientDtos;
        }

        public async Task<ClientDTO?> GetByIdAsync(Guid id)
        {
            // Получаем клиента по его ID
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
                return null;

            // Преобразуем клиента в DTO
            var clientDto = _mapper.Map<ClientDTO>(client);

            // Подтягиваем дополнительные данные в зависимости от типа клиента
            if (client.IsIndividual)
            {
                var individual = await _context.Individuals.FirstOrDefaultAsync(i => i.ClientId == id);
                if (individual != null)
                {
                    clientDto.Name = $"{individual.FirstName} {individual.LastName}";
                }
            }
            else
            {
                var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.ClientId == id);
                if (organization != null)
                {
                    clientDto.Name = organization.CompanyName;
                }
            }

            return clientDto;
        }

        public async Task CreateAsync(ClientDTO clientDto)
        {
            // Создаем базового клиента
            var client = _mapper.Map<Client>(clientDto);
            client.Id = Guid.NewGuid();

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // В зависимости от типа клиента добавляем данные в соответствующую таблицу
            if (clientDto.IsIndividual)
            {
                var individual = new Individual
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    FirstName = clientDto.Name?.Split(' ')[0] ?? string.Empty,
                    LastName = clientDto.Name?.Split(' ').Skip(1).FirstOrDefault() ?? string.Empty
                };
                _context.Individuals.Add(individual);
            }
            else
            {
                var organization = new Organization
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    CompanyName = clientDto.Name ?? string.Empty
                };
                _context.Organizations.Add(organization);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ClientDTO clientDto)
        {
            // Получаем клиента из базы
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == clientDto.Id);
            if (client == null)
                throw new Exception("Client not found");

            // Обновляем данные клиента
            _mapper.Map(clientDto, client);
            _context.Clients.Update(client);

            // Обновляем данные в зависимости от типа клиента
            if (clientDto.IsIndividual)
            {
                var individual = await _context.Individuals.FirstOrDefaultAsync(i => i.ClientId == clientDto.Id);
                if (individual == null)
                {
                    // Если данных нет, создаем новую запись
                    individual = new Individual
                    {
                        Id = Guid.NewGuid(),
                        ClientId = clientDto.Id,
                        FirstName = clientDto.Name?.Split(' ')[0] ?? string.Empty,
                        LastName = clientDto.Name?.Split(' ').Skip(1).FirstOrDefault() ?? string.Empty
                    };
                    _context.Individuals.Add(individual);
                }
                else
                {
                    // Если данные есть, обновляем
                    individual.FirstName = clientDto.Name?.Split(' ')[0] ?? string.Empty;
                    individual.LastName = clientDto.Name?.Split(' ').Skip(1).FirstOrDefault() ?? string.Empty;
                    _context.Individuals.Update(individual);
                }
            }
            else
            {
                var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.ClientId == clientDto.Id);
                if (organization == null)
                {
                    // Если данных нет, создаем новую запись
                    organization = new Organization
                    {
                        Id = Guid.NewGuid(),
                        ClientId = clientDto.Id,
                        CompanyName = clientDto.Name ?? string.Empty
                    };
                    _context.Organizations.Add(organization);
                }
                else
                {
                    // Если данные есть, обновляем
                    organization.CompanyName = clientDto.Name ?? string.Empty;
                    _context.Organizations.Update(organization);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            // Удаляем базового клиента
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
                throw new Exception("Client not found");

            // Удаляем данные из таблиц Individual или Organization
            if (client.IsIndividual)
            {
                var individual = await _context.Individuals.FirstOrDefaultAsync(i => i.ClientId == id);
                if (individual != null)
                {
                    _context.Individuals.Remove(individual);
                }
            }
            else
            {
                var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.ClientId == id);
                if (organization != null)
                {
                    _context.Organizations.Remove(organization);
                }
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }
}
