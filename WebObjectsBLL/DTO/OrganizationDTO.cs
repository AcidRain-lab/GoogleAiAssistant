namespace WebObjectsBLL.DTO
{
    public class OrganizationDTO
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public string CompanyName { get; set; } = null!;

        public string RegistrationNumber { get; set; } = null!;

        public string? TaxId { get; set; }

        public string? Industry { get; set; }

        public int? NumberOfEmployees { get; set; }

        public decimal? AnnualTurnover { get; set; }

        public Guid? ContactPersonId { get; set; }
    }
}
