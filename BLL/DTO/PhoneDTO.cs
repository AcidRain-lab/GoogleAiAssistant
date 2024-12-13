using BLL.DTO.Phones;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class PhoneDTO
    {
        public Guid? Id { get; set; }
       // [Required(ErrorMessage = "Please enter phone number.")]
        [RegularExpression(@"^\d{3} \d{3} \d{4}$", ErrorMessage = "Please enter valid phone number")]
        public string Number { get; set; } = string.Empty;

       //[Range(1, int.MaxValue, ErrorMessage = "Please select a valid phone type.")]
        [Display(Name = "Phone Type")]
        public int PhoneTypeId { get; set; }

        public bool IsPrimary { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        //public List<PhoneTypeSummaryDTO> PhoneTypeList { get; set; } = new() { new PhoneTypeSummaryDTO { PhoneTypeId = 0, Name = "Select Phone Type" } };
        public List<PhoneTypeSummaryDTO> PhoneTypeList { get; set; } = new() { new PhoneTypeSummaryDTO () };
    }
}
