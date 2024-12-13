using DAL.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using BLL.DTO.Addresses;
using BLL.DTO.TradeTypes;
using BLL.DTO.LeadSources;
using BLL.DTO.WorkTypes;
using BLL.DTO.JobCategories;
using BLL.DTO.Phones;
using BLL.Attributes;
using BLL.DTO.MediaData;

namespace BLL.DTO
{
    public class LeadDTO : AvatarDTO, IMediaGallery
    {
        public bool IsNewRecord { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter email address."), EmailAddress(ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid lead source.")]
        [Display(Name = "Lead Source")]
        public int LeadSourceId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid job category.")]


        [Display(Name = "Job Category")]
        public int JobCategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid work type.")]
        [Display(Name = "Work Type")]
        public int WorkTypeId { get; set; }
        public Guid Id { get; set; } // Уникальный идентификатор для лидов, генерируется системой
        public DateTime? DateCreation { get; set; } // Дата создания, устанавливается при создании лида
        public string CompanyName { get; set; }
        public string CrossReference { get; set; }

        // Идентификаторы адресов
        public Guid? LocationAddressId { get; set; }
        public Guid? MailingAddressId { get; set; }
        public Guid? BillingAddressId { get; set; }

        // Поля для UI
         [ValidatePhoneList(ErrorMessage = "Please enter atleast one phone number.")]
        public List<PhoneDTO> Phones { get; set; } = new();
        public List<WorkTypeDTO> WorkTypeList { get; set; } = new() { new WorkTypeDTO { WorkTypeId = 0, Name = "Select Work Type" } };
        public List<LeadSourceDTO> LeadSourceList { get; set; } = new() { new LeadSourceDTO { LeadSourceId = 0, Name = "Select Lead Source" } };
        public List<TradeTypeDTO> TradeTypeList { get; set; } = new() { new TradeTypeDTO { Id = 0, Name = "Select Trade Type" } };
        // Индекс для определения основного телефона
        
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
        public List<MediaDatum> MediaData { get; set; } = new List<MediaDatum>();

        public AddressSummaryDTO LocationAddress { get; set; } = new();
        public AddressSummaryDTO MailingAddress { get; set; } = new();
        public AddressSummaryDTO BillingAddress { get; set; } = new();
        public List<JobCategorySummaryDTO> JobCategoriesList { get; set; } = new() { new JobCategorySummaryDTO { JobCategoryId = 0, Name = "Select Job Category" } };
        public List<PhoneTypeSummaryDTO> PhoneTypeList { get; set; } = new() { new PhoneTypeSummaryDTO () };
        //public List<PhoneTypeSummaryDTO> PhoneTypeList { get; set; } = new() { new PhoneTypeSummaryDTO { PhoneTypeId = 0, Name = "Select Phone Type" } };

    }


}
