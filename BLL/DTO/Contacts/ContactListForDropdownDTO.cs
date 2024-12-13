namespace BLL.DTO.Contacts
{
    public class ContactListForDropdownDTO
    {
        public string Email { get; set; } = string.Empty;

        public Guid ContactId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Base64Image { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
