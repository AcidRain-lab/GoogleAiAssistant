namespace BLL.DTO.Leads
{
    public class LeadSummaryDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CrossReference { get; set; } = string.Empty;
        public string CompanyJobTitle { get; set; } = string.Empty;
        public Guid LocationAddressId { get; set; } = Guid.Empty;

        public Guid? MailingAddressId { get; set; } = Guid.Empty;

        public Guid? BillingAddressId { get; set; } = Guid.Empty;
    }
}
