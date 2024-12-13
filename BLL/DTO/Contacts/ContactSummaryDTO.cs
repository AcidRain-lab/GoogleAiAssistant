
namespace BLL.DTO.Contacts
{
    public class ContactSummaryDTO
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Guid Id { get; set; }

        public string CompanyName { get; set; } = string.Empty;
        public string Initials
        {
            get
            {
                return (!string.IsNullOrEmpty(FirstName) ? FirstName[0].ToString() : "") +
                       (!string.IsNullOrEmpty(LastName) ? LastName[0].ToString() : "");
            }
        }

    }
}
