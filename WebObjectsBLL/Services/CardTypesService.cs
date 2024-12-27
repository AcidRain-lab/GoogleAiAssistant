using AutoMapper;
using DAL.Models;
using MediaLib.DTO;
using MediaLib.Helpers;
using MediaLib.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly DocumentService _documentService;

        public CardTypesService(
            BankContext context,
            IMapper mapper,
            AvatarService avatarService,
            MediaGalleryService mediaGalleryService,
            DocumentService documentService)
        {
            _context = context;
            _mapper = mapper;
            _avatarService = avatarService;
            _mediaGalleryService = mediaGalleryService;
            _documentService = documentService;
        }

        public async Task<IEnumerable<CardTypeTableDTO>> GetAllWithAvatarsAsync()
        {
            var cardTypes = await _context.CardTypes
                .Include(ct => ct.PaymentSystemType)
                .ToListAsync();

            var result = new List<CardTypeTableDTO>();

            foreach (var ct in cardTypes)
            {
                var primaryMedia = await _mediaGalleryService.GetPrimaryMediaAsync(ct.Id);

                result.Add(new CardTypeTableDTO
                {
                    Id = ct.Id,
                    Name = ct.Name,
                    Description = ct.Description,
                    PaymentSystemTypeName = ct.PaymentSystemType?.Name,
                    Avatar = await _avatarService.GetAvatarAsync(ct.Id),
                    PrimaryMedia = primaryMedia
                });
            }

            return result;
        }

        public async Task<CardTypeDetailDTO> GetByIdWithDetailsAsync(Guid id)
        {
            var cardType = await _context.CardTypes
                .Include(ct => ct.PaymentSystemType)
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (cardType == null)
                throw new KeyNotFoundException("Card type not found");

            var dto = _mapper.Map<CardTypeDetailDTO>(cardType);
            dto.Avatar = await _avatarService.GetAvatarAsync(id);
            dto.MediaFiles = await _mediaGalleryService.GetMediaDataListAsync(id);
            dto.Documents = await _documentService.GetDocumentsListAsync(id);

            return dto;
        }

        public async Task AddAsync(
            CardTypeDetailDTO cardTypeDto,
            AvatarDTO? avatar,
            List<MediaDataDTO>? mediaFiles,
            List<DocumentsDTO>? documents)
        {
            var cardType = _mapper.Map<CardType>(cardTypeDto);
            _context.CardTypes.Add(cardType);
            await _context.SaveChangesAsync();

            if (avatar != null)
            {
                avatar.AssociatedRecordId = cardType.Id;
                await _avatarService.SetAvatarAsync(avatar);
            }

            if (mediaFiles != null)
            {
                await _mediaGalleryService.ManageMediaAsync(
                    cardType.Id,
                    null,
                    null,
                    null,
                    MediaLib.ObjectType.CardType);
            }

            if (documents != null)
            {
                await _documentService.ManageDocumentsAsync(
                    cardType.Id,
                    null,
                    null,
                    null,
                    MediaLib.ObjectType.CardType);
            }
        }

        public async Task UpdateAsync(
            CardTypeDetailDTO cardTypeDto,
            AvatarDTO? avatar,
            List<IFormFile>? newMediaFiles,
            List<IFormFile>? newDocumentFiles,
            Guid? primaryMediaId,
            List<Guid>? mediaToDelete,
            Guid? primaryDocumentId,
            List<Guid>? documentsToDelete)
        {
            var cardType = await _context.CardTypes.FirstOrDefaultAsync(ct => ct.Id == cardTypeDto.Id);
            if (cardType == null)
                throw new KeyNotFoundException("Card type not found");

            _mapper.Map(cardTypeDto, cardType);
            await _context.SaveChangesAsync();

            if (avatar != null)
            {
                avatar.AssociatedRecordId = cardType.Id;
                await _avatarService.SetAvatarAsync(avatar);
            }

            await _mediaGalleryService.ManageMediaAsync(
                cardType.Id,
                newMediaFiles,
                mediaToDelete,
                primaryMediaId,
                MediaLib.ObjectType.CardType);

            await _documentService.ManageDocumentsAsync(
                cardType.Id,
                newDocumentFiles,
                documentsToDelete,
                primaryDocumentId,
                MediaLib.ObjectType.CardType);
        }

        public async Task DeleteAsync(Guid id)
        {
            var cardType = await _context.CardTypes.FirstOrDefaultAsync(ct => ct.Id == id);
            if (cardType == null)
                throw new KeyNotFoundException("Card type not found");

            _context.CardTypes.Remove(cardType);
            await _context.SaveChangesAsync();

            await _avatarService.RemoveAvatarAsync(id);
            await _mediaGalleryService.RemoveMediaByRecordIdAsync(id);
            await _documentService.RemoveDocumentsByRecordIdAsync(id);
        }

        public async Task DeleteMediaAsync(Guid mediaId)
        {
            await _mediaGalleryService.RemoveMediaAsync(mediaId);
        }

        public async Task DeleteDocumentAsync(Guid documentId)
        {
            await _documentService.RemoveDocumentAsync(documentId);
        }
    }
}
