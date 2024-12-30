using MediaLib.DTO;

namespace WebObjectsBLL.DTO
{
    public class ClientDetailDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string PassportData { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public AvatarDTO? Avatar { get; set; }
    }
}
