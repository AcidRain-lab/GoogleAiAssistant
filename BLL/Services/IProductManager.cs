using BLL.DTO;
using BLL.DTO.Common;
using BLL.DTO.Contacts;

namespace BLL.Services
{
    public interface IProductManager
    {
        public Task<ServiceResult> EditCreate(ContactDTO model, List<Guid>? removedImageIds);
       
        Task<ContactDTO> GetContactDataForEditById(Guid id);

        Task<ContactDTO> GetShortContactDataById(Guid id); 
        Task<ContactDTO> GetContactsDropdownData();
        Task<bool> Delete(Guid id);
        Task<List<ContactListForDropdownDTO>> GetContactsList();
        Task<List<string>> GetContactsEmailList(List<Guid> contactIds);
    }
}
