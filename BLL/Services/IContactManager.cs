using BLL.DTO;
using BLL.DTO.Common;
using BLL.DTO.Contacts;

namespace BLL.Services
{
    public interface IContactManager
    {
        public Task<ServiceResult> EditCreateContact(ContactDTO model, List<Guid>? removedImageIds);
        public Task<ServiceResult> ConvertLeadToContact(Guid id);
        Task<ContactDTO> GetContactDataForEditById(Guid id);

        Task<ContactDTO> GetShortContactDataById(Guid id); 
        Task<ContactDTO> GetContactsDropdownData();
        Task<bool> Delete(Guid id);
        Task<List<ContactListForDropdownDTO>> GetContactsList();
        Task<List<string>> GetContactsEmailList(List<Guid> contactIds);
    }
}
