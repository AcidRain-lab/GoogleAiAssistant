using AutoMapper;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class CardTypesService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public CardTypesService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Получить все типы карт
        public async Task<IEnumerable<CardTypeDTO>> GetAllAsync()
        {
            var cardTypes = await _context.CardTypes
                .Include(ct => ct.PaymentSystemType) // Включить связанные данные
                .ToListAsync();

            return _mapper.Map<IEnumerable<CardTypeDTO>>(cardTypes);
        }

        // Получить тип карты по ID
        public async Task<CardTypeDTO> GetByIdAsync(Guid id)
        {
            var cardType = await _context.CardTypes
                .Include(ct => ct.PaymentSystemType)
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (cardType == null)
                throw new KeyNotFoundException("Card type not found");

            return _mapper.Map<CardTypeDTO>(cardType);
        }

        // Добавить новый тип карты
        public async Task AddAsync(CardTypeDTO cardTypeDto)
        {
            var cardType = _mapper.Map<CardType>(cardTypeDto);
            _context.CardTypes.Add(cardType);
            await _context.SaveChangesAsync();
        }

        // Обновить существующий тип карты
        public async Task UpdateAsync(CardTypeDTO cardTypeDto)
        {
            var cardType = await _context.CardTypes.FirstOrDefaultAsync(ct => ct.Id == cardTypeDto.Id);
            if (cardType == null)
                throw new KeyNotFoundException("Card type not found");

            _mapper.Map(cardTypeDto, cardType);
            await _context.SaveChangesAsync();
        }

        // Удалить тип карты
        public async Task DeleteAsync(Guid id)
        {
            var cardType = await _context.CardTypes.FirstOrDefaultAsync(ct => ct.Id == id);
            if (cardType == null)
                throw new KeyNotFoundException("Card type not found");

            _context.CardTypes.Remove(cardType);
            await _context.SaveChangesAsync();
        }
    }
}
