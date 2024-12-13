using BLL.Services;
using DAL.Models;
using BLL.DTO;
using Microsoft.EntityFrameworkCore;
using BLL.DTO.Common;
using BLL.DTO.Addresses;
using DAL.Repositories;
using BLL.Services.Implementations;
using BLL.DTO.Phones;
using Microsoft.Identity.Client;
using BLL.DTO.Leads;
using BLL.DTO.ContactTypes;
using BLL.DTO.States;
using BLL.DTO.Countries;
using BLL.DTO.Contacts;

namespace BLL
{
    public class ContactManager : IContactManager
    {
        private readonly CrmContext _context;
        private readonly ICountryRepository _countryRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IPhoneTypesRepository _phoneTypesRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly ILeadRepository _leadRepository;
        private readonly IMediaDataRepository _mediaDataRepository;
        private readonly IAvatarRepository _avatarRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IContactTypeRepository _contactTypeRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IContactTypeListRepository _contactTypeListRepository;
        public ContactManager(CrmContext context, ICountryRepository countryRepository, IContactRepository contactRepository,
            IPhoneTypesRepository phoneTypesRepository, IPhoneRepository phoneRepository, ILeadRepository leadRepository,
            IMediaDataRepository mediaDataRepository, IAvatarRepository avatarRepository, ILocationRepository locationRepository,
            IContactTypeRepository contactTypeRepository, IStateRepository stateRepository, IContactTypeListRepository contactTypeListRepository)
        {
            _context = context;
            _countryRepository = countryRepository;
            _contactRepository = contactRepository;
            _phoneTypesRepository = phoneTypesRepository;
            _phoneRepository = phoneRepository;
            _leadRepository = leadRepository;
            _mediaDataRepository = mediaDataRepository;
            _avatarRepository = avatarRepository;
            _locationRepository = locationRepository;
            _contactTypeRepository = contactTypeRepository;
            _stateRepository = stateRepository;
            _contactTypeListRepository = contactTypeListRepository;
        }


        public async Task<ContactDTO> GetContactsDropdownData()
        {
            var phoneTypeList = await _phoneTypesRepository.GetPhoneTypesList();
            var stateList = await _stateRepository.GetStatesList();
            var contactTypeList = await _contactTypeRepository.GetContactTypeList();
            var countryList = await _countryRepository.GetCountriesList();
            var leadList = await _leadRepository.GetLeadsList();
            var contactTypesListIds = await _contactTypeListRepository.GetContactTypeIdsList();
            List<LeadListForDropdownDTO> leads = new();
            if (leadList.Any())
            {
                leadList.ForEach(x =>
                {
                    leads.Add(new LeadListForDropdownDTO()
                    {
                        LeadId = x.Id,
                        Email = x.Email,
                    });
                });
            }
            List<PhoneTypeSummaryDTO> phoneTypes = new();
            if (phoneTypeList.Any())
            {
                phoneTypeList.ForEach(x =>
                {
                    phoneTypes.Add(new PhoneTypeSummaryDTO()
                    {
                        Name = x.Name,
                        PhoneTypeId = x.Id
                    });
                });
            }
            List<ContactTypeDTO> contactTypes = new();
            {
                contactTypeList.ForEach(x =>
                {
                    contactTypes.Add(new ContactTypeDTO()
                    {
                        ContactId = x.Id,
                        Name = x.Name,
                    });
                });
            }
            List<StateDTO> states = new();
            if (stateList.Any())
            {
                stateList.ForEach(x =>
                {
                    states.Add(new StateDTO()
                    {
                        StateId = x.Id,
                        Name = x.Name,
                    });
                });
            }
            List<CountryDTO> countries = new();
            if (countryList.Any())
            {
                countryList.ForEach(x =>
                {
                    countries.Add(new CountryDTO()
                    {
                        CountryId = x.Id,
                        Name = x.Name,
                    });
                });
            }
            var contact = new ContactDTO
            {

                PhoneTypeList = phoneTypes,
                ContactTypeList = contactTypes,
                StateList = states,
                CountryList = countries,
                LeadList = leads,
                ContactTypeIdsList = contactTypesListIds,
            };
            return contact;
        }

        public async Task<List<Contact>> GetAllContacts()
        {
            return await _context.Contacts.Include(c => c.ContactsTypesLists).ToListAsync();
        }

        public async Task<(bool success, string errorMessage)> CreateContact(ContactDTO model)
        {
            var contact = new Contact
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                CompanyName = model.CompanyName,
                CrossReference = model.CrossReference,
                CompanyJobTitle = model.CompanyJobTitle,
                LeadId = model.LeadId,
              
            };

            if (_context.Contacts.Any(u => u.Email == contact.Email))
            {
                return (false, "Email already exists.");
            }
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            List<Phone> phones = AddOrUpdatePhoneListForContact(model, contact);
            bool isPhoneListAddedOrUpdated = await _phoneRepository.AddOrUpdatePhoneList(phones);

            // Сохраняем все изменения, включая лид, связанные адреса и телефоны
            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                foreach (var file in model.ImageFiles)
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        var imageData = stream.ToArray();

                        // Create MediaData object for the image
                        var mediaData = new MediaDatum
                        {
                            Id = Guid.NewGuid(), // Generate a new Id for the media data
                            Name = file.FileName,
                            Extension = Path.GetExtension(file.FileName),
                            Content = imageData,
                            AssociatedRecordId = contact.Id, // Associate with the lead
                            IsPrime = false, // Set as non-primary for additional images
                            ObjectTypeId = 1 // Assuming Image is an enumerated type representing media types
                        };

                        // Save media data to the database
                        await _context.MediaData.AddAsync(mediaData);
                    }
                }

                await _context.SaveChangesAsync();
            }
            var location = new Location
            {
                Street = model.MailingAddress.Street ?? String.Empty,
                SuiteAptUnit = model.MailingAddress.SuiteAptUnit ?? String.Empty,
                City = model.MailingAddress.City ?? String.Empty,
                Zip = model.MailingAddress.Zip,
                StateId = model.MailingAddress.StateId,
                CountryId = model.MailingAddress.CountryId,
            };
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();
            contact.MailingAddressId = location.Id;

            var Billinglocation = new Location
            {
                Street = model.BillingAddress.Street ?? String.Empty,
                SuiteAptUnit = model.BillingAddress.SuiteAptUnit ?? String.Empty,
                City = model.BillingAddress.City ?? String.Empty,
                Zip = model.BillingAddress.Zip,
                StateId = model.BillingAddress.StateId,
                CountryId = model.BillingAddress.CountryId,
            };
            await _context.Locations.AddAsync(Billinglocation);
            await _context.SaveChangesAsync();
            contact.BilllingAddressId = Billinglocation.Id;


            foreach (var selectedTypeId in model.ContactIds)
            {
                var contactTypeList = new ContactsTypesList
                {
                    ContactId = contact.Id,
                    ContactTypeId = selectedTypeId
                };

                await _context.ContactsTypesLists.AddAsync(contactTypeList);
            }

            await _context.SaveChangesAsync();
            return (true, "");
        }


        public async Task<(bool success, string errorMessage)> UpdateContact(Guid id, ContactDTO model, List<Guid> removedImageIds)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(l => l.Id == id);

            if (contact == null)
            {
                // Contact not found
                return (false, "not found.");
            }

            // Update contact information
            contact.FirstName = model.FirstName;
            contact.LastName = model.LastName;
            contact.Email = model.Email;
            contact.CompanyName = model.CompanyName;
            contact.CrossReference = model.CrossReference;
            contact.CompanyJobTitle = model.CompanyJobTitle;
            contact.LeadId = model.LeadId;
            // Update other fields as necessary
            try
            {

                //if (_context.Contacts.Any(u => u.Email == contact.Email && u.Id != contact.Id))
                //{
                //    // Handle email already exists scenario
                //    return (false, "Email already exists.");
                //}

                _context.Update(contact);

                // Remove phones that are not in the model.LeadsPhones list
                foreach (var existingPhone in contact.Phones.ToList())
                {
                    if (!model.Phones.Any(p => p.Id == existingPhone.Id && p.Id != null))
                    {
                        _context.Phones.Remove(existingPhone);
                    }
                }

                //prepare phone list to add or update 

                List<Phone> phones = AddOrUpdatePhoneListForContact(model, contact);
                bool isPhoneListddedOrUpdated = await _phoneRepository.AddOrUpdatePhoneList(phones);
                if (removedImageIds != null)
                {
                    foreach (var Dltid in removedImageIds)
                    {
                        var mediaData = _context.MediaData.FirstOrDefault(m => m.Id == Dltid);
                        if (mediaData != null)
                        {
                            _context.MediaData.Remove(mediaData);
                        }
                    }
                    _context.SaveChanges();
                }


                if (model.ImageFiles != null && model.ImageFiles.Count > 0)
                {
                    foreach (var file in model.ImageFiles)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await file.CopyToAsync(stream);
                            var imageData = stream.ToArray();

                            // Create MediaData object for the image
                            var mediaData = new MediaDatum
                            {
                                Id = Guid.NewGuid(), // Generate a new Id for the media data
                                Name = file.FileName,
                                Extension = Path.GetExtension(file.FileName),
                                Content = imageData,
                                AssociatedRecordId = contact.Id, // Associate with the lead
                                IsPrime = false, // Set as non-primary for additional images
                                ObjectTypeId = 1 // Assuming Image is an enumerated type representing media types
                            };

                            // Save media data to the database
                            await _context.MediaData.AddAsync(mediaData);
                        }
                    }

                    await _context.SaveChangesAsync();
                }

                // Update location

                var mailingAddres = await _context.Locations.FindAsync(contact.MailingAddressId);
                if (mailingAddres != null)
                {
                    mailingAddres.Street = model.MailingAddress.Street ?? String.Empty;
                    mailingAddres.SuiteAptUnit = model.MailingAddress.SuiteAptUnit ?? String.Empty;
                    mailingAddres.City = model.MailingAddress.City ?? String.Empty;
                    mailingAddres.Zip = model.MailingAddress.Zip;
                    mailingAddres.StateId = model.MailingAddress.StateId;
                    mailingAddres.CountryId = model.MailingAddress.CountryId;
                }

                var billingAddres = await _context.Locations.FindAsync(contact.BilllingAddressId);
                if (billingAddres != null)
                {
                    billingAddres.Street = model.BillingAddress.Street ?? String.Empty;
                    billingAddres.SuiteAptUnit = model.BillingAddress.SuiteAptUnit ?? String.Empty;
                    billingAddres.City = model.BillingAddress.City ?? String.Empty;
                    billingAddres.Zip = model.BillingAddress.Zip;
                    billingAddres.StateId = model.BillingAddress.StateId;
                    billingAddres.CountryId = model.BillingAddress.CountryId;
                }
                await _context.SaveChangesAsync();

                // Retrieve existing ContactTypesList entries for the contact
                var existingEntries = await _context.ContactsTypesLists
                    .Where(ct => ct.ContactId == contact.Id)
                    .ToListAsync();

                // Get the list of selected contact type IDs from the model
                var selectedTypeIds = model.ContactIds ?? new List<int>();

                // Remove existing entries that are not selected
                var entriesToRemove = existingEntries.Where(entry => !selectedTypeIds.Contains(entry.ContactTypeId)).ToList();
                _context.ContactsTypesLists.RemoveRange(entriesToRemove);

                // Add new entries for selected contact type IDs that are not already associated with the contact
                foreach (var selectedTypeId in selectedTypeIds)
                {
                    if (!existingEntries.Any(entry => entry.ContactTypeId == selectedTypeId))
                    {
                        var newEntry = new ContactsTypesList
                        {
                            ContactId = contact.Id,
                            ContactTypeId = selectedTypeId
                        };
                        _context.ContactsTypesLists.Add(newEntry);
                    }
                }

                await _context.SaveChangesAsync();

                return (true, "");
            }
            catch (DbUpdateException ex)
            {
                return (false, "Error updating contact.");
            }
        }


        public async Task<bool> Delete(Guid id)
        {
            return await _contactRepository.Delete(id);
        }

        public async Task<ServiceResult> EditCreateContact(ContactDTO model, List<Guid>? removedImageIds)
        {
            if (model.Id == null)
            {
                if (_context.Contacts.Any(u => u.Email == model.Email))
                {
                    return new ServiceResult(ServiceResultStatus.Error, "Email already exists.");
                }
                var createResult = await CreateContact(model);
                return new ServiceResult(ServiceResultStatus.Success);
            }
            else
            {
                var createResult = await UpdateContact((Guid)model.Id, model, removedImageIds);
                return new ServiceResult(ServiceResultStatus.Success);
            }
        }

        public async Task<ServiceResult> ConvertLeadToContact(Guid id)
        {
            var leadDetail = await _leadRepository.GetLeadById(id);
            if (leadDetail == null)
            {
                return new ServiceResult(ServiceResultStatus.NotFound, "Record not found.");
            }
            var phoneList = await _phoneRepository.GetPhoneListByAssociatedId(id);
            var billingAddress = await _locationRepository.GetLocationById(leadDetail.BillingAddressId ?? Guid.Empty);
            var mailingAddress = await _locationRepository.GetLocationById(leadDetail.MailingAddressId ?? Guid.Empty);
            var locationAddress = await _locationRepository.GetLocationById(id);
            var mediaDataList = await _mediaDataRepository.GetMediaDataListByAssociatedRecordId(id);
            var avatar = await _avatarRepository.GetAvatarByAssociatedRecordId(id);
            //save contact as lead
            bool emailExists = await _contactRepository.EmailExists(leadDetail.Email);
            if (emailExists)
            {
                return new ServiceResult(ServiceResultStatus.Error, "Email address already exists for contact.");
            }
            Contact contact = new()
            {
                FirstName = leadDetail.FirstName,
                LastName = leadDetail.LastName,
                Email = leadDetail.Email,
                CompanyName = leadDetail.CompanyName,
                CrossReference = leadDetail.CrossReference,
                CompanyJobTitle = "",
                LeadId = id,
            };
            Guid newContactId = await _contactRepository.Save(contact);
            if (newContactId == Guid.Empty)
            {
                return new ServiceResult(ServiceResultStatus.Error, "Contact not saved, please try again.");
            }
            ContactsTypesList contactTypeList = new ContactsTypesList
            {
                ContactId = newContactId,
                ContactTypeId = 1
            };
            await _contactTypeListRepository.Save(contactTypeList);
            if (phoneList.Any())
            {
                List<Phone> phones = new List<Phone>();
                phoneList.ForEach(phone =>
                {
                    phones.Add(new Phone()
                    {
                        AssociatedRecordId = newContactId,
                        PhoneTypeId = phone.PhoneTypeId,
                        Number = phone.Number,
                        IsPrimary = phone.IsPrimary,
                        Id = Guid.Empty
                    });
                });
                await _phoneRepository.AddOrUpdatePhoneList(phones);
            }
            //save lead media data as contact media data
            if (mediaDataList.Any())
            {
                List<MediaDatum> mediaData = new List<MediaDatum>();
                mediaDataList.ForEach(media =>
                {
                    mediaData.Add(new MediaDatum()
                    {
                        Id = Guid.NewGuid(),
                        Name = media.Name,
                        Extension = media.Extension,
                        Content = media.Content,
                        AssociatedRecordId = newContactId,
                        IsPrime = false,
                        ObjectTypeId = 1
                    });
                });
                await _mediaDataRepository.SaveMediaDataList(mediaData);
            }
            //save lead locations as contact locations
            if (mailingAddress != null)
            {
                Location mailingLocation = SetLocationParams(mailingAddress);
                Guid mailingAddressId = await _locationRepository.Save(mailingLocation);
                contact.MailingAddressId = mailingAddressId;
            }
            if (billingAddress != null)
            {
                Location billingLocation = SetLocationParams(billingAddress);
                Guid billingAddressId = await _locationRepository.Save(billingLocation);
                contact.BilllingAddressId = billingAddressId;
            }
            await _contactRepository.Update(contact);
            //save lead avatar as contact avatar
            if (avatar != null)
            {
                Avatar contactAvatar = new()
                {
                    AssociatedRecordId = newContactId,
                    Content = avatar.Content,
                    Extension = avatar.Extension,
                    Name = avatar.Name,
                    ObjectTypeId = avatar.ObjectTypeId,
                };
                await _avatarRepository.Save(contactAvatar);
            }
            return new ServiceResult(ServiceResultStatus.Success);

        }


        public async Task<ContactDTO> GetShortContactDataById(Guid id)
        {
            var contactSummary = await _contactRepository.GetContactById(id);
           
            var avatar = await _avatarRepository.GetAvatarByAssociatedRecordId(id);

            ContactDTO contactDTO = new()
            {
                Id = contactSummary.Id,
                FirstName = contactSummary.FirstName,
                LastName = contactSummary.LastName,
                Email = contactSummary.Email,
                CompanyName = contactSummary.CompanyName ?? "",

              
               
                LeadId = contactSummary.LeadId
               
             
            };
          
            if (avatar != null)
            {
                contactDTO.Base64Image = Convert.ToBase64String(avatar.Content);
            }
            return contactDTO;
        }
        public async Task<ContactDTO> GetContactDataForEditById(Guid id)
        {
            var contactSummary = await _contactRepository.GetContactById(id);
            var mediaDataList = await _mediaDataRepository.GetMediaDataListByAssociatedRecordId(id);
            var phoneList = await _phoneRepository.GetPhoneListByAssociatedId(id);
            var avatar = await _avatarRepository.GetAvatarByAssociatedRecordId(id);
            var phoneTypeList = await _phoneTypesRepository.GetPhoneTypesList();

            var mailingAddress = await _locationRepository.GetLocationById(contactSummary.MailingAddressId ?? Guid.Empty);
            var billingAddress = await _locationRepository.GetLocationById(contactSummary.BilllingAddressId ?? Guid.Empty);
            var leadList = await _leadRepository.GetLeadsList();
            var contactData = await GetContactsDropdownData();
            var contactTypesListIds = await _contactTypeListRepository.GetContactTypeIdsListByContactId(id);
            List<PhoneTypeSummaryDTO> phoneTypes = new();
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
                        PhoneTypeList = phoneTypes
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
                    PhoneTypeList = phoneTypes
                });
            }

            ContactDTO contactDTO = new()
            {
                Id = contactSummary.Id,
                FirstName = contactSummary.FirstName,
                LastName = contactSummary.LastName,
                Email = contactSummary.Email,
                CompanyName = contactSummary.CompanyName ?? "",
                CrossReference = contactSummary.CrossReference ?? "",
                MediaData = mediaDataList,
                PhoneTypeList = phoneTypes,
                Phones = phoneDTOList,
                ContactTypeList = contactData.ContactTypeList,
                LeadList = contactData.LeadList,
                LeadId = contactSummary.LeadId,
                ContactTypeIdsList = contactTypesListIds,
                ContactIds = contactData.ContactIds,
                CompanyJobTitle = contactSummary.CompanyJobTitle
            };

            if (billingAddress != null)
            {

                contactDTO.BillingAddress = new AddressSummaryDTO()
                {
                    Id = billingAddress!.Id,
                    City = billingAddress.City,
                    CountryId = billingAddress.CountryId,
                    StateId = billingAddress.StateId,
                    Street = billingAddress.Street,
                    SuiteAptUnit = billingAddress.SuiteAptUnit,
                    Zip = billingAddress.Zip.GetValueOrDefault(),
                    StateList = contactData.StateList,
                    CountryList = contactData.CountryList
                };
            }
            if (mailingAddress != null)
            {

                contactDTO.MailingAddress = new AddressSummaryDTO()
                {
                    Id = mailingAddress!.Id,
                    City = mailingAddress.City,
                    CountryId = mailingAddress.CountryId,
                    StateId = mailingAddress.StateId,
                    Street = mailingAddress.Street,
                    SuiteAptUnit = mailingAddress.SuiteAptUnit,
                    Zip = mailingAddress.Zip.GetValueOrDefault(),
                    StateList = contactData.StateList,
                    CountryList = contactData.CountryList
                };
            }

            if (avatar != null)
            {
                contactDTO.Base64Image = Convert.ToBase64String(avatar.Content);
            }
            return contactDTO;
        }
        public async Task<List<ContactListForDropdownDTO>> GetContactsList()
        {
            var contactList = await _contactRepository.GetContactsList();
            var avatarList = await _avatarRepository.GetAll();
            List<ContactListForDropdownDTO> contacts = new();
            if (contactList.Any())
            {
                contactList.ForEach(contact =>
                {
                    contacts.Add(new ContactListForDropdownDTO()
                    {
                        ContactId = contact.Id,
                        Email = contact.Email,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        FullName = contact.FirstName + " " + contact.LastName
                    });
                });
            }
            if (contacts.Any())
            {
                if (avatarList.Any())
                {
                    contacts.ForEach(user =>
                    {
                        if (avatarList.Any(x => x.AssociatedRecordId == user.ContactId))
                        {
                            var content = avatarList.FirstOrDefault(x => x.AssociatedRecordId == user.ContactId)!.Content;
                            if (content != null)
                            {
                                user.Base64Image = Convert.ToBase64String(content);
                            }
                        }
                    });
                }

            }
            return contacts;
        }

        public async Task<List<string>> GetContactsEmailList(List<Guid> contactIds)
        {
            var contactList = await _contactRepository.GetContactsListByIds(contactIds);
            List<string> emails = new();
            string email = string.Empty;
            if (contactList.Any())
            {
                contactList.ForEach(contact =>
                {
                    email = contact.Email;
                    emails.Add(email);
                });
            }

            return emails;
        }



        #region Private Methods
        private static List<Phone> AddOrUpdatePhoneListForContact(ContactDTO model, Contact? contact)
        {
            List<Phone> phones = new List<Phone>();
            if (model!.Phones.Any())
            {
                model.Phones.ForEach(x =>
                {
                    phones.Add(new Phone()
                    {
                        Id = x.Id ?? Guid.Empty,
                        Number = x.Number,
                        PhoneTypeId = x.PhoneTypeId,
                        IsPrimary = x.IsPrimary,
                        AssociatedRecordId = contact.Id
                    });
                });
            }

            return phones;
        }
        private static Location SetLocationParams(Location? input)
        {
            Location location = new()
            {
                Street = input!.Street,
                SuiteAptUnit = input.SuiteAptUnit,
                City = input.City,
                Zip = input.Zip,
                StateId = input.StateId,
                CountryId = input.CountryId,

            };
            return location;
        }
        #endregion
    }
}
