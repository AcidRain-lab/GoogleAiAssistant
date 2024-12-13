using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class ContactRepository : IContactRepository
    {
        private readonly CrmContext _context;
        public ContactRepository(CrmContext context)
        {
            _context = context;
        }

        public async Task<List<Contact>> GetContactsList()
        {
            var contactList = await _context.Contacts.Include(ctype => ctype.ContactsTypesLists).ToListAsync().ConfigureAwait(false);
            return contactList;
        }

        public async Task<Contact?> GetContactById(Guid id)
        {
            var contact = await _context.Contacts.Where(con => con.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
            return contact;
        }
        public async Task<(bool success, string message)> ConvertLeadToContact(Guid id)
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
            //TODO Get Avatars
            //TODO Get Contacts

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

        public async Task<Guid> Save(Contact contact)
        {
            await _context.AddAsync(contact).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            if (contact != null)
            {
                return contact.Id;
            }
            return Guid.Empty;
        }
        public async Task<int> Update(Contact contact)
        {
            _context.Update(contact);
            return (await _context.SaveChangesAsync().ConfigureAwait(false));
        }
        public async Task<bool> EmailExists(string email) => await _context.Contacts.AnyAsync(x => x.Email == email).ConfigureAwait(false);

        public async Task<bool> Delete(Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<Contact?> GetContactByEmail(string email)
        {
            var contact = await _context.Contacts.Where(con => con.Email == email).FirstOrDefaultAsync().ConfigureAwait(false);
            return contact;
        }

        public async Task<List<Contact>> GetContactsListByIds(List<Guid> contactGuids)
        {
            var contactList = await _context.Contacts.Where(x => contactGuids.Contains(x.Id)).ToListAsync().ConfigureAwait(false);
            return contactList;
        }
    }
}
