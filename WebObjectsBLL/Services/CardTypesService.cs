using AutoMapper;
using DAL.Models;
using MediaLib.DTO;
using MediaLib.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.Services
{
    public class CardTypesService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;
        private readonly AvatarService _avatarService;
        private readonly MediaGalleryService _mediaGalleryService;

        public CardTypesService(BankContext context, IMapper mapper, AvatarService avatarService, MediaGalleryService mediaGalleryService)
        {
            _context = context;
            _mapper = mapper;
            _avatarService = avatarService;
            _mediaGalleryService = mediaGalleryService;
        }

        public async Task<IEnumerable<CardTypeDTO>> GetAllAsync()
        {
            var cardTypes = await _context.CardTypes
                .Include(ct => ct.PaymentSystemType)
                .ToListAsync();

            var cardTypeDtos = _mapper.Map<IEnumerable<CardTypeDTO>>(cardTypes);

            foreach (var dto in cardTypeDtos)
            {
                dto.AvatarBase64 = await _avatarService.GetAvatarBase64Async(dto.Id);
            }

            return cardTypeDtos;
        }

        
        public async Task AddAsync(CardTypeDTO cardTypeDto, AvatarDTO? avatar, List<IFormFile>? mediaFiles)
        {
            var cardType = _mapper.Map<CardType>(cardTypeDto);
            _context.CardTypes.Add(cardType);
            await _context.SaveChangesAsync();

            if (avatar != null)
            {
                await _avatarService.SetAvatarAsync(avatar, cardType.Id, "CardType");
            }

            if (mediaFiles != null && mediaFiles.Any())
            {
                await _mediaGalleryService.AddMediaAsync(cardType.Id, mediaFiles, "CardType");
            }
        }

        public async Task UpdateAsync(CardTypeDTO cardTypeDto, AvatarDTO? avatar, List<IFormFile>? mediaFiles)
        {
            var cardType = await _context.CardTypes.FirstOrDefaultAsync(ct => ct.Id == cardTypeDto.Id);
            if (cardType == null)
                throw new KeyNotFoundException("Тип карты не найден");

            _mapper.Map(cardTypeDto, cardType);
            await _context.SaveChangesAsync();

            if (avatar != null)
            {
                await _avatarService.SetAvatarAsync(avatar, cardType.Id, "CardType");
            }

            if (mediaFiles != null && mediaFiles.Any())
            {
                await _mediaGalleryService.AddMediaAsync(cardType.Id, mediaFiles, "CardType");
            }
        }

        public async Task<CardTypeDTO> GetByIdAsync(Guid id)
        {
            var cardType = await _context.CardTypes
                .Include(ct => ct.PaymentSystemType)
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (cardType == null)
                throw new KeyNotFoundException("Тип карты не найден");

            var cardTypeDto = _mapper.Map<CardTypeDTO>(cardType);
            cardTypeDto.AvatarBase64 = await _avatarService.GetAvatarBase64Async(cardTypeDto.Id);
            cardTypeDto.MediaFiles = await _mediaGalleryService.GetMediaDataListAsync(cardTypeDto.Id);

            return cardTypeDto;
        }

        public async Task DeleteAsync(Guid id)
        {
            var cardType = await _context.CardTypes.FirstOrDefaultAsync(ct => ct.Id == id);
            if (cardType == null)
                throw new KeyNotFoundException("Тип карты не найден");

            _context.CardTypes.Remove(cardType);
            await _context.SaveChangesAsync();

            await _avatarService.SetAvatarAsync(null, id, "CardType");
        }


        public async Task<IEnumerable<CardTypeDTO>> GetAllWithAvatarsAsync()
        {
            var cardTypes = await _context.CardTypes
                .Include(ct => ct.PaymentSystemType)
                .ToListAsync();

            var cardTypeDTOs = cardTypes.Select(ct =>
            {
                var dto = _mapper.Map<CardTypeDTO>(ct);

                // Retrieve the associated avatar
                var avatar = _context.Avatars
                    .FirstOrDefault(a => a.AssociatedRecordId == ct.Id && a.ObjectTypeId == 1);

                // Ensure the avatar is not null and contains content
                if (avatar != null && avatar.Content != null)
                {
                    // Assuming the Avatar model has a 'Content' (byte[]) property
                    // MIME type is hardcoded as 'image/jpeg'. Adjust if needed.
                    dto.AvatarBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(avatar.Content)}";
                }

                return dto;
            }).ToList();

            return cardTypeDTOs;
        }


    }
}

