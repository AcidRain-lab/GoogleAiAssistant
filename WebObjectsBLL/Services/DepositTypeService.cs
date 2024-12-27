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
    public class DepositTypeService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;
        private readonly AvatarService _avatarService;
        private readonly MediaGalleryService _mediaGalleryService;
        private readonly DocumentService _documentService;

        public DepositTypeService(
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

        public async Task<IEnumerable<DepositTypeTableDTO>> GetAllWithAvatarsAsync()
        {
            var depositTypes = await _context.DepositTypes
                .Include(dt => dt.DepositTerms)
                .ToListAsync();

            var result = new List<DepositTypeTableDTO>();

            foreach (var dt in depositTypes)
            {
                var primaryMedia = await _mediaGalleryService.GetPrimaryMediaAsync(dt.Id);

                result.Add(new DepositTypeTableDTO
                {
                    Id = dt.Id,
                    DepositName = dt.DepositName,
                    Description = dt.Description,
                    Avatar = await _avatarService.GetAvatarAsync(dt.Id),
                    PrimaryMedia = primaryMedia
                });
            }

            return result;
        }
        public async Task<IEnumerable<DepositTypeDetailDTO>> GetAllWithDetailsAsync()
        {
            var depositTypes = await _context.DepositTypes
                .Include(dt => dt.DepositTerms)
                .ToListAsync();

            var result = new List<DepositTypeDetailDTO>();

            foreach (var dt in depositTypes)
            {
                var dto = _mapper.Map<DepositTypeDetailDTO>(dt);
                dto.Avatar = await _avatarService.GetAvatarAsync(dt.Id);
                dto.MediaFiles = await _mediaGalleryService.GetMediaDataListAsync(dt.Id);
                dto.Documents = await _documentService.GetDocumentsListAsync(dt.Id);

                result.Add(dto);
            }

            return result;
        }

        /*public async Task<DepositTypeDetailDTO> GetByIdWithDetailsAsync(Guid id)
        {
            var depositType = await _context.DepositTypes
                .Include(dt => dt.DepositTerms)
                .FirstOrDefaultAsync(dt => dt.Id == id);

            if (depositType == null)
                throw new KeyNotFoundException("Deposit type not found");

            var dto = _mapper.Map<DepositTypeDetailDTO>(depositType);
            dto.Avatar = await _avatarService.GetAvatarAsync(id);
            dto.MediaFiles = await _mediaGalleryService.GetMediaDataListAsync(id);
            dto.Documents = await _documentService.GetDocumentsListAsync(id);

            return dto;
        }*/
        public async Task<DepositTypeDetailDTO> GetByIdWithDetailsAsync(Guid id)
        {
            var depositType = await _context.DepositTypes
                .Include(dt => dt.DepositTerms)
                .FirstOrDefaultAsync(dt => dt.Id == id);

            if (depositType == null)
                throw new KeyNotFoundException("Deposit type not found");

            var dto = _mapper.Map<DepositTypeDetailDTO>(depositType);
            dto.Avatar = await _avatarService.GetAvatarAsync(id);
            dto.MediaFiles = await _mediaGalleryService.GetMediaDataListAsync(id);
            dto.Documents = await _documentService.GetDocumentsListAsync(id);

            return dto;
        }

        public async Task AddAsync(
     DepositTypeDetailDTO depositTypeDto,
     AvatarDTO? avatar,
     List<MediaDataDTO>? mediaFiles,
     List<DocumentsDTO>? documents,
     Guid ownerId)
        {
            var depositType = _mapper.Map<DepositType>(depositTypeDto);

            // Добавляем коллекцию терминов
            foreach (var termDto in depositTypeDto.DepositTerms)
            {
                var term = _mapper.Map<DepositTerm>(termDto);
                depositType.DepositTerms.Add(term);
            }

            _context.DepositTypes.Add(depositType);
            await _context.SaveChangesAsync();

            // Управление аватаром
            if (avatar != null)
            {
                avatar.AssociatedRecordId = depositType.Id;
                avatar.ObjectTypeId = (int)MediaLib.ObjectType.DepositType;
                await _avatarService.SetAvatarAsync(avatar);
            }

            // Добавляем медиа-файлы
            if (mediaFiles != null)
            {
                foreach (var media in mediaFiles)
                {
                    media.AssociatedRecordId = depositType.Id;
                    media.ObjectTypeId = (int)MediaLib.ObjectType.DepositType;
                    media.OwnerId = ownerId;
                }
                await _mediaGalleryService.AddMediaAsync(mediaFiles);
            }

            // Добавляем документы
            if (documents != null)
            {
                foreach (var document in documents)
                {
                    document.AssociatedRecordId = depositType.Id;
                    document.ObjectTypeId = (int)MediaLib.ObjectType.DepositType;
                    document.OwnerId = ownerId;
                }
                await _documentService.AddDocumentsAsync(documents);
            }
        }


        public async Task UpdateAsync(
     DepositTypeDetailDTO depositTypeDto,
     AvatarDTO? avatar,
     List<IFormFile>? newMediaFiles,
     List<IFormFile>? newDocumentFiles,
     Guid? primaryMediaId,
     List<Guid>? mediaToDelete,
     Guid? primaryDocumentId,
     List<Guid>? documentsToDelete,
     Guid ownerId)
        {
            // Получаем существующую запись
            var depositType = await _context.DepositTypes
                .Include(dt => dt.DepositTerms) // Подгружаем связанные коллекции
                .FirstOrDefaultAsync(dt => dt.Id == depositTypeDto.Id);
            if (depositType == null)
                throw new KeyNotFoundException("Deposit type not found");

            // Маппинг DTO в модель
            _mapper.Map(depositTypeDto, depositType);

            // Обновляем коллекцию DepositTerms
            depositType.DepositTerms.Clear();
            foreach (var term in depositTypeDto.DepositTerms)
            {
                depositType.DepositTerms.Add(_mapper.Map<DepositTerm>(term));
            }

            await _context.SaveChangesAsync();

            // Управление аватаром
            if (avatar != null)
            {
                avatar.AssociatedRecordId = depositType.Id;
                avatar.ObjectTypeId = (int)MediaLib.ObjectType.DepositType;
                await _avatarService.SetAvatarAsync(avatar);
            }

            // Управление медиа-файлами
            await _mediaGalleryService.ManageMediaAsync(
                depositType.Id,
                newMediaFiles,
                mediaToDelete,
                primaryMediaId,
                MediaLib.ObjectType.DepositType,
                ownerId);

            // Управление документами
            await _documentService.ManageDocumentsAsync(
                depositType.Id,
                newDocumentFiles,
                documentsToDelete,
                primaryDocumentId,
                MediaLib.ObjectType.DepositType,
                ownerId);
        }


        public async Task DeleteAsync(Guid id)
        {
            var depositType = await _context.DepositTypes.FirstOrDefaultAsync(dt => dt.Id == id);
            if (depositType == null)
                throw new KeyNotFoundException("Deposit type not found");

            _context.DepositTypes.Remove(depositType);
            await _context.SaveChangesAsync();

            await _avatarService.RemoveAvatarAsync(id);
            await _mediaGalleryService.RemoveMediaByRecordIdAsync(id);
            await _documentService.RemoveDocumentsByRecordIdAsync(id);
        }
    }
}
