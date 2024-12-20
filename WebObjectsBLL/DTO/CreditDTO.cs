using System.ComponentModel.DataAnnotations;

namespace WebObjectsBLL.DTO
{
    public class CreditDTO
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Credit amount must be greater than 0.")]
        public decimal CreditAmount { get; set; }

        [Required(ErrorMessage = "Currency is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be 3 characters long.")]
        public string Currency { get; set; } = string.Empty;

        [Range(0.01, 100.0, ErrorMessage = "Interest rate must be between 0.01 and 100.")]
        public double InterestRate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
