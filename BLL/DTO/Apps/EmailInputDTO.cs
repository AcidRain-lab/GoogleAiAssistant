
using BLL.DTO.Contacts;
using BLL.DTO.Emails;
using System.Security.Principal;

namespace BLL.DTO.Apps
{
    public class EmailInputDTO
    {
        public List<ContactListForDropdownDTO> ContactsList { get; set; } = new();
        public EmailMessagesInputDTO EmailMessages { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;
    }

}
