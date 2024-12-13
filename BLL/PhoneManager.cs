// RoleManager.cs in BLL namespace

using DAL.Models;
using Microsoft.EntityFrameworkCore;
using BLL.DTO;
using Newtonsoft.Json;
using BLL.Services;
using DAL.Repositories;
using Microsoft.Identity.Client;

namespace BLL
{

    // напиши функцию которая будет возвращать список телефонов по ID обьекта
    

    public class PhoneManager : IPhoneManager
    {
        private readonly CrmContext _context;
        private readonly IPhoneRepository _phoneRepository;

        public PhoneManager(CrmContext context, IPhoneRepository phoneRepository)
        {
            _context = context;
            _phoneRepository = phoneRepository;
        }

        // напиши функцию которая будет возвращать список телефонов в формате DTO по ID обьекта
        //public GetAuthorizationRequestUrlParameterBuilder<PhoneDTO> GetPhonesDTO(Guid objectId)
        //{
        //    var existingPhonesList = GetPhones(objectId);
        //    var phoneDTOList = new List<PhoneDTO>();

        //    foreach (var phone in existingPhonesList)
        //    {
        //        phoneDTOList.Add(new PhoneDTO
        //        {
        //            Id = phone.Id,
        //            Number = phone.Number,
        //            PhoneTypeId = phone.PhoneTypeId,
        //            IsPrimary = (bool)phone.IsPrimary
        //        });
        //    }

        //    return phoneDTOList;
        //}

        public List<PhoneDTO> GetPhonesDTO(Guid objectId, LeadDTO model)
        {
            model.Phones = GetPhones(objectId).Select(lp => new PhoneDTO
            {
                Id = lp.Id,
                Number = lp.Number,
                PhoneTypeId = lp.PhoneTypeId,
                IsPrimary = (bool)lp.IsPrimary
            }).ToList();


            return model.Phones;
        }

       
        public List<PhoneDTO> GetPhonesDTOForEdit(Guid objectId, LeadDTO model)
        {
            model.Phones = GetPhonesDTO(objectId, model);

            if (model.Phones.Count == 0)
            {
                model.Phones = new List<PhoneDTO> { new PhoneDTO { Number = "", IsPrimary = true } };
            }

            return model.Phones;
        }

        public List<Phone> GetPhones(Guid objectId)
        {
            var existingPhonesList = _context.Phones.Where(p => p.AssociatedRecordId == objectId).ToList();
            return existingPhonesList;
        }


        public async Task<string> GetPhoneTypesList()
        {
            var phoneTypeDtos = await _context.PhoneTypes
                                             .Select(pt => new PhoneTypeDTO { Id = pt.Id, Name = pt.Name })
                                             .ToListAsync();
            // Сериализация списка DTO в JSON
            string phoneTypesJson = JsonConvert.SerializeObject(phoneTypeDtos);

            return phoneTypesJson;

        }
        public void EditObjectsPhonesList(Guid objectId, List<PhoneDTO> model)
        {

            model.RemoveAll(phone => string.IsNullOrEmpty(phone.Number));
            // existingPhonesList  из базы для нашего обьекта
            var existingPhonesList = GetPhones(objectId);

            // model.Phones - DTO обьект со страницы редактирования
            // вытираем те телефонф которых нет в списке, возможно удалили
            // Remove phones that are not in the model.LeadsPhones list



            foreach (var existingPhone in existingPhonesList)
            {

                if (!model.Any(p => p.Id == existingPhone.Id && p.Id != null))
                {
                    _context.Phones.Remove(existingPhone);
                }
            }

            // Update or add phones based on the model.LeadsPhones list
            if (model != null && model.Any())
            {
                // model.Phones - DTO обьект со страницы редактирования
                foreach (var phoneDto in model)
                {
                    var existingPhone = existingPhonesList.FirstOrDefault(p => p.Id == phoneDto.Id);

                    //bool? IsPrimaryNumberFromView = model.PrimaryPhoneIndex != null && model.Phones.IndexOf(phoneDto) == model.PrimaryPhoneIndex;

                    if (existingPhone != null)
                    {
                        // Update existing phone
                        existingPhone.Number = phoneDto.Number;
                        existingPhone.IsPrimary = phoneDto.IsPrimary;
                        existingPhone.PhoneTypeId = phoneDto.PhoneTypeId;
                        _context.Phones.Update(existingPhone); // Mark as updated
                    }
                    else
                    {
                        // Add new phone
                        var phone = new Phone
                        {
                            Number = phoneDto.Number,
                            IsPrimary = phoneDto.IsPrimary,
                            PhoneTypeId = phoneDto.PhoneTypeId,
                            AssociatedRecordId = objectId
                        };

                        _context.Phones.Add(phone);
                    }
                }

            }
            //_context.SaveChangesAsync();


        }

        public async Task<List<PhoneDTO>> GetPhoneListByAssociatedId(Guid associatedId)
        {
            var phoneList = await _phoneRepository.GetPhoneListByAssociatedId(associatedId);
            List<PhoneDTO> phoneListDTO = new();
            if (phoneList.Any())
            {
                phoneList.ForEach(x =>
                {
                    phoneListDTO.Add(new PhoneDTO()
                    {
                        Id = x.Id,
                        IsPrimary = x.IsPrimary ?? false,
                        PhoneTypeId = x.PhoneTypeId,
                        Number = x.Number,
                    });
                });
            }
            return phoneListDTO;
        }
        public async Task<bool> CreateUpdatePhoneByAssociatedId(Guid associatedId, List<PhoneDTO> phoneList)
        {
            List<Phone> phones = new List<Phone>();
            if (phoneList.Any())
            {
                phoneList.ForEach(x =>
                {
                    phones.Add(new Phone()
                    {
                        Id = x.Id ?? Guid.Empty,
                        Number = x.Number,
                        PhoneTypeId = x.PhoneTypeId,
                        IsPrimary = x.IsPrimary,
                        AssociatedRecordId = associatedId
                    });
                });
            }
            bool addedOrUpdated = await _phoneRepository.AddOrUpdatePhoneList(phones);
            return addedOrUpdated;
        }

        public async Task<bool> DeletePhonesByIds(List<Guid> phoneIds)
        {
            return await _phoneRepository.DeletePhonesByIds(phoneIds);
        }
    }
}
