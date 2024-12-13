using BLL.DTO.Contacts;
using BLL.DTO.JobCategories;
using BLL.DTO.WorkTypes;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Estimates
{
    public class EstimatesInputDTO
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Contact is required.")]
        [Display(Name = "Contact")]
        public Guid ContactId { get; set; }

        public string? Notes { get; set; }

        public Guid? OwnerId { get; set; }

        public int? StatusId { get; set; }

        public int CalculateTypeId { get; set; } = 1;//As it is required so for now maiking it hardcoded as we don't have any screen to add CalculateType

        [Required(ErrorMessage = "Square is required.")]
        [RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage = "Please enter a valid number.")]
        public decimal Square { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Job category is required.")]

        [Display(Name = "Job Category")]
        public int JobCategoryId { get; set; }

        [Required(ErrorMessage = "Work type is required.")]
        [Display(Name = "Work Type")]
        public int WorkTypeId { get; set; }
        public decimal? PriceMaterial { get; set; }

        public decimal? PriceLabor { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? Text { get; set; }
        public string? FullName { get; set; } = string.Empty;
        public bool IsNewRecord { get; set; } = false;
        public List<Guid> TradeIds { get; set; } = new();
        public List<Guid> TradeTemplateIds { get; set; } = new();
        public List<WorkTypeDTO> WorkTypeList { get; set; } = new();
        public List<JobCategorySummaryDTO> JobCategoriesList { get; set; } = new();
        public List<ContactListForDropdownDTO> ContactsList { get; set; } = new();
    }
}
