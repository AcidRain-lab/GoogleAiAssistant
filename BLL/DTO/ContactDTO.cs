using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using BLL.DTO;
using BLL.DTO.Addresses;
using BLL.Attributes;
using BLL.DTO.Phones;
using BLL.DTO.ContactTypes;
using BLL.DTO.Leads;
using BLL.DTO.States;
using BLL.DTO.Countries;

namespace BLL.DTO
{
    public class LeadIdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || (Guid)value == Guid.Empty)
            {
                return new ValidationResult("Please select a valid lead.");
            }

            return ValidationResult.Success;
        }
    }
    public class ContactDTO : AvatarDTO, IMediaGallery
    {

        public bool IsNewRecord { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter email address."), EmailAddress(ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }


        public Guid? Id { get; set; } // Уникальный идентификатор для лидов, генерируется системой
        public string CompanyName { get; set; }
        public string? CompanyJobTitle { get; set; } = string.Empty;
        public string CrossReference { get; set; }

        public Guid? LocationAddressId { get; set; }
        public Guid? MailingAddressId { get; set; }
        public Guid? BillingAddressId { get; set; }


        [ValidatePhoneList(ErrorMessage = "Please enter atleast one phone number.")]
        public List<PhoneDTO> Phones { get; set; } = new List<PhoneDTO>();


        [Required(ErrorMessage = "Please select a valid lead.")]
        [LeadIdValidation]
        [Display(Name = "Lead")]
        public Guid? LeadId { get; set; }

        //public int? PrimaryPhoneIndex { get; set; }

        public int? IsSameAddress { get; set; }

        // Идентификаторы адресов


        [Required(ErrorMessage = "Please select a valid lead source.")]
        [Display(Name = "Lead Source")]
        public List<int> ContactIds { get; set; }

        public List<MediaDatum> MediaData { get; set; } = new List<MediaDatum>();
        public List<LeadListForDropdownDTO> LeadList { get; set; } = new();

        // Индекс для определения основного телефона
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();

        // Поля для UI

        public List<ContactTypeDTO> ContactTypeList { get; set; } = new();
        public List<PhoneTypeSummaryDTO> PhoneTypeList { get; set; } = new() { new PhoneTypeSummaryDTO { PhoneTypeId = 0, Name = "Select Phone Type" } };
        public List<StateDTO> StateList { get; set; } = new();
        public List<CountryDTO> CountryList { get; set; } = new();
        public AddressSummaryDTO BillingAddress { get; set; } = new();
        public AddressSummaryDTO MailingAddress { get; set; } = new();
        public List<int> ContactTypeIdsList { get; set; } = new();


    }


}
