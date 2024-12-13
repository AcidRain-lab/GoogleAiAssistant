using BLL.Services;
using DAL.Models;
using BLL.DTO;
using Microsoft.EntityFrameworkCore;
using BLL.DTO.Addresses;
using BLL.DTO.Leads;
using DAL.Repositories;
using BLL.DTO.Phones;
using System.Numerics;
using Microsoft.AspNetCore.Http;

namespace BLL
{
    public class LeadManager : ILeadManager
    {
        private readonly CrmContext _context;
        private readonly ILeadRepository _leadRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IMediaDataRepository _mediaDataRepository;
        private readonly IAvatarRepository _avatarRepository;
        private readonly IPhoneTypesRepository _phoneTypesRepository;
        private readonly ILocationRepository _locationRepository;
        public LeadManager(CrmContext context, ILeadRepository leadRepository, IPhoneRepository phoneRepository,
            IMediaDataRepository mediaDataRepository, IAvatarRepository avatarRepository,
            IPhoneTypesRepository phoneTypesRepository, ILocationRepository locationRepository)
        {
            _context = context;
            _leadRepository = leadRepository;
            _phoneRepository = phoneRepository;
            _mediaDataRepository = mediaDataRepository;
            _avatarRepository = avatarRepository;
            _phoneTypesRepository = phoneTypesRepository;
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Lead>> GetAllLeads()
        {
            var leads = await _context.Leads.ToListAsync();
            return leads;
        }
        public async Task<Lead> GetLeadById(Guid id)
        {
            var lead = await _context.Leads
                .FirstOrDefaultAsync(m => m.Id == id);

            var phone = await _context.Phones
                         .Where(m => m.AssociatedRecordId == id)
                         .ToListAsync();

            var loc = await _context.Locations
                         .Where(m => m.Id == lead.LocationAddressId)
                         .ToListAsync();
            var mailAdd = await _context.Locations
                         .Where(m => m.Id == lead.MailingAddressId)
                         .ToListAsync();
            var bilAdd = await _context.Locations
                         .Where(m => m.Id == lead.BillingAddressId)
                         .ToListAsync();
            var img = await _context.MediaData
                         .Where(m => m.AssociatedRecordId == lead.Id)
                         .ToListAsync();

            return lead;
        }

        public async Task<LeadDTO> GetAllData()
        {
            var lead = new LeadDTO
            {



            };

            return lead;
        }
        public async Task<(bool success, string errorMessage)> EditCreate(LeadDTO model, List<Guid> removedImageIds)
        {

            Lead lead;
            if (model.IsNewRecord) //Create
            {
                model.IsNewRecord = true;
                lead = new Lead();
            }
            else
            {
                lead = await _context.Leads.FindAsync(model.Id);
                if (lead == null)
                {
                    return (false, "");
                }
            }


            lead.FirstName = model.FirstName;
            lead.LastName = model.LastName;
            lead.Email = model.Email;
            lead.LeadSourceId = model.LeadSourceId;
            lead.JobCategoryId = model.JobCategoryId;
            lead.WorkTypeId = model.WorkTypeId;
            lead.CompanyName = model.CompanyName;
            lead.CrossReference = model.CrossReference;

            try
            {
                if (model.IsNewRecord)
                {
                    if (_context.Leads.Any(u => u.Email == lead.Email && u.Id != lead.Id))
                    {
                        return (false, "Email already exists.");
                    }
                    _context.Add(lead);
                }
                else
                {
                    _context.Update(lead);
                }
                await _context.SaveChangesAsync();

                PhoneManager phoneManager = new PhoneManager(_context, _phoneRepository);
                phoneManager.EditObjectsPhonesList(lead.Id, model.Phones);

                var mediaService = new MediaService(_context);
                await mediaService.SetAvatarAsync(model, lead.Id, "Image");
                await mediaService.SetGalleryAsync(lead.Id, model.ImageFiles, removedImageIds, "Image");

                await UpdateOrCreateAddressAsync(_context, lead, model);

                await _context.SaveChangesAsync();
                return (true, "");
            }
            catch (DbUpdateException ex)
            {
                return (false, "");
            }
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null)
            {
                // User not found
                return false;
            }
            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<(bool success, string errorMessage)> ConvertToContact(Guid id)
        {
            var lead = await _context.Leads
                .FirstOrDefaultAsync(m => m.Id == id);
            var leadphone = await _context.Phones
                         .Where(m => m.AssociatedRecordId == id)
                         .ToListAsync();

            var loc = await _context.Locations
                         .Where(m => m.Id == lead.LocationAddressId)
                         .ToListAsync();
            var mailAdd = await _context.Locations
                         .Where(m => m.Id == lead.MailingAddressId)
                         .ToListAsync();
            var bilAdd = await _context.Locations
                         .Where(m => m.Id == lead.BillingAddressId)
                         .ToListAsync();
            var img = await _context.MediaData
                         .Where(m => m.AssociatedRecordId == lead.Id)
                         .ToListAsync();

            if (lead != null)
            {
                var contact = new Contact
                {
                    FirstName = lead.FirstName,
                    LastName = lead.LastName,
                    Email = lead.Email,
                    CompanyName = lead.CompanyName,
                    CrossReference = lead.CrossReference,
                    CompanyJobTitle = "",
                    LeadId = id,
                    // Дополните другими полями по необходимости
                };
                if (_context.Contacts.Any(u => u.Email == contact.Email))
                {
                    return (false, "Email already exists.");
                }
                await _context.Contacts.AddAsync(contact);
                await _context.SaveChangesAsync();
                if (lead.Phones != null && lead.Phones.Any())
                {
                    foreach (var phoneDto in lead.Phones)
                    {
                        var phone = new Phone
                        {
                            Number = phoneDto.Number,
                            IsPrimary = phoneDto.IsPrimary,
                            PhoneTypeId = phoneDto.PhoneTypeId,
                            AssociatedRecordId = contact.Id // ID лида будет назначен после сохранения
                        };

                        contact.Phones.Add(phone); // Добавляем телефон напрямую к лиду, используя навигационное свойство
                    }
                }
                if (lead.MediaData != null && lead.MediaData.Count > 0)
                {
                    foreach (var file in lead.MediaData)
                    {

                        // Create MediaData object for the image
                        var mediaData = new MediaDatum
                        {
                            Id = Guid.NewGuid(), // Generate a new Id for the media data
                            Name = file.Name,
                            Extension = file.Extension,
                            Content = file.Content,
                            AssociatedRecordId = contact.Id, // Associate with the lead
                            IsPrime = false, // Set as non-primary for additional images
                            ObjectTypeId = 1 // Assuming Image is an enumerated type representing media types
                        };

                        // Save media data to the database
                        await _context.MediaData.AddAsync(mediaData);

                    }

                    await _context.SaveChangesAsync();
                }
                if (lead.MailingAddress != null)
                {
                    var location = new Location
                    {
                        Street = lead.MailingAddress.Street ?? String.Empty,
                        SuiteAptUnit = lead.MailingAddress.SuiteAptUnit ?? String.Empty,
                        City = lead.MailingAddress.City ?? String.Empty,
                        Zip = lead.MailingAddress.Zip,
                        StateId = lead.MailingAddress.StateId,
                        CountryId = lead.MailingAddress.CountryId,
                    };
                    await _context.Locations.AddAsync(location);
                    await _context.SaveChangesAsync();
                    contact.MailingAddressId = location.Id;
                }
                if (lead.BillingAddress != null)
                {
                    var Billinglocation = new Location
                    {
                        Street = lead.BillingAddress.Street ?? String.Empty,
                        SuiteAptUnit = lead.BillingAddress.SuiteAptUnit ?? String.Empty,
                        City = lead.BillingAddress.City ?? String.Empty,
                        Zip = lead.BillingAddress.Zip,
                        StateId = lead.BillingAddress.StateId,
                        CountryId = lead.BillingAddress.CountryId,
                    };
                    await _context.Locations.AddAsync(Billinglocation);
                    await _context.SaveChangesAsync();
                    contact.BilllingAddressId = Billinglocation.Id;
                }
                var contactTypeList = new ContactsTypesList
                {
                    ContactId = contact.Id,
                    ContactTypeId = 1
                };
                await _context.ContactsTypesLists.AddAsync(contactTypeList);
                await _context.SaveChangesAsync();
            }
            return (true, "");

        }

        public async Task<LeadDTO> GetLeadDataForEditById(Guid id)
        {
            var leadSummary = await _leadRepository.GetLeadById(id);
            var mediaDataList = await _mediaDataRepository.GetMediaDataListByAssociatedRecordId(id);
            var phoneList = await _phoneRepository.GetPhoneListByAssociatedId(id);
            var avatar = await _avatarRepository.GetAvatarByAssociatedRecordId(id);
            var phoneTypeList = await _phoneTypesRepository.GetPhoneTypesList();
            var locationAddress = await _locationRepository.GetLocationById(leadSummary!.LocationAddressId ?? Guid.Empty);
            var mailingAddress = await _locationRepository.GetLocationById(leadSummary.MailingAddressId ?? Guid.Empty);
            var billingAddress = await _locationRepository.GetLocationById(leadSummary.BillingAddressId ?? Guid.Empty);

            //List<PhoneTypeSummaryDTO> phoneTypes = new() { new PhoneTypeSummaryDTO { PhoneTypeId = 0, Name = "Select Phone Type" } }; ;
            List<PhoneTypeSummaryDTO> phoneTypes = new() { new PhoneTypeSummaryDTO () }; ;
            if (phoneTypeList.Any())
            {
                phoneTypeList.ForEach(pt =>
                {
                    phoneTypes.Add(new PhoneTypeSummaryDTO()
                    {
                        Name = pt.Name,
                        PhoneTypeId = pt.Id
                    });
                });
            }
            List<PhoneDTO> phoneDTOList = new List<PhoneDTO>();
            if (phoneList.Any())
            {
                phoneList.ForEach(phone =>
                {
                    phoneDTOList.Add(new PhoneDTO()
                    {
                        Id = phone.Id,
                        PhoneTypeId = phone.PhoneTypeId,
                        Number = phone.Number,
                        IsPrimary = phone.IsPrimary ?? false,
                        PhoneTypeList= phoneTypes
                    });
                });

            }
            else
            {
                phoneDTOList.Add(new PhoneDTO()
                {
                    PhoneTypeId = 0,
                    Number = "",
                    IsPrimary = false,
                    PhoneTypeList= phoneTypes
                });
            }

            LeadDTO leadDTO = new()
            {
                Id = leadSummary.Id,
                FirstName = leadSummary.FirstName,
                LastName = leadSummary.LastName,
                Email = leadSummary.Email,
                CompanyName = leadSummary.CompanyName ?? "",
                CrossReference = leadSummary.CrossReference ?? "",
                JobCategoryId = leadSummary.JobCategoryId,
                WorkTypeId = leadSummary.WorkTypeId,
                LeadSourceId = leadSummary.LeadSourceId,
                MediaData = mediaDataList,
                PhoneTypeList = phoneTypes,
                Phones = phoneDTOList,

            };
            if (locationAddress != null)
            {
                leadDTO.LocationAddress = new AddressSummaryDTO()
                {
                    Id = locationAddress!.Id,
                    City = locationAddress.City,
                    CountryId = locationAddress.CountryId,
                    StateId = locationAddress.StateId,
                    Street = locationAddress.Street,
                    SuiteAptUnit = locationAddress.SuiteAptUnit,
                    Zip = locationAddress.Zip.GetValueOrDefault(),
 
            };
            }
            if (billingAddress != null)
            {

                leadDTO.BillingAddress = new AddressSummaryDTO()
                {
                    Id = billingAddress!.Id,
                    City = billingAddress.City,
                    CountryId = billingAddress.CountryId,
                    StateId = billingAddress.StateId,
                    Street = billingAddress.Street,
                    SuiteAptUnit = billingAddress.SuiteAptUnit,
                    Zip = billingAddress.Zip.GetValueOrDefault(),
                };
            }
            if (mailingAddress != null)
            {

                leadDTO.MailingAddress = new AddressSummaryDTO()
                {
                    Id = mailingAddress!.Id,
                    City = mailingAddress.City,
                    CountryId = mailingAddress.CountryId,
                    StateId = mailingAddress.StateId,
                    Street = mailingAddress.Street,
                    SuiteAptUnit = mailingAddress.SuiteAptUnit,
                    Zip = mailingAddress.Zip.GetValueOrDefault(),
                };
            }

            if (avatar != null)
            {
                leadDTO.Base64Image = Convert.ToBase64String(avatar.Content);
            }
            return leadDTO;
        }

        private async Task UpdateOrCreateAddressAsync(CrmContext context, Lead lead, LeadDTO model)
        {
            Location locationAddress = new Location()
            {

            };
            await UpdateOrCreateLocationAsync(context, model.LocationAddress, (id) => lead.LocationAddressId = id);
            await UpdateOrCreateLocationAsync(context, model.MailingAddress, (id) => lead.MailingAddressId = id);
            await UpdateOrCreateLocationAsync(context, model.BillingAddress, (id) => lead.BillingAddressId = id);
        }

        private async Task UpdateOrCreateLocationAsync(CrmContext context, AddressSummaryDTO location, Action<Guid> setId)
        {
            Location existingLocation = null;
            if (location?.Id != null && location.Id != Guid.Empty)
            {
                existingLocation = await context.Locations.FirstOrDefaultAsync(l => l.Id == location.Id);
            }

            if (existingLocation != null)
            {
                context.Entry(existingLocation).CurrentValues.SetValues(location);
            }
            else
            {
                if (location != null)
                {
                    Location newLocations = new Location()
                    {
                        City = location.City,
                        Street = location.Street,
                        StateId = location.StateId,
                        CountryId = location.CountryId,
                        SuiteAptUnit = location.SuiteAptUnit,
                        Zip = location.Zip,
                    };
                    context.Locations.Add(newLocations);
                    await context.SaveChangesAsync();
                    setId(newLocations.Id);
                }
            }
        }

        public async Task<List<LeadListForDropdownDTO>> GetAllLeadsList()
        {
            var leadList = await _leadRepository.GetLeadsList();
            List<LeadListForDropdownDTO> leadLists = new();
            if (leadList.Any())
            {
                leadList.ForEach(le =>
                {
                    LeadListForDropdownDTO lead = new()
                    {
                        Email = le.Email,
                        LeadId = le.Id,
                    };
                    leadLists.Add(lead);
                });
            }
            return leadLists;
        }

        public async Task<LeadSummaryDTO?> GetLeadByGuidId(Guid id)
        {
            var leadSummary = await _leadRepository.GetLeadById(id);
            LeadSummaryDTO? leadSummaryDTO = null;
            if (leadSummary != null)
            {
                leadSummaryDTO = new();
                leadSummaryDTO.FirstName = leadSummary.FirstName ?? "";
                leadSummaryDTO.LastName = leadSummary.LastName;
                leadSummaryDTO.Email = leadSummary.Email;
                leadSummaryDTO.Id = leadSummary.Id;
                leadSummaryDTO.CompanyName = leadSummary.CompanyName ?? "";
            }
            return leadSummaryDTO;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _leadRepository.EmailExists(email);
        }


    }
}
