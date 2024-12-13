using BLL.DTO.Contacts;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Appointments
{
    public class AppointmentsInputDTO
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [Required]
        [RegularExpression("^((?!00000000-0000-0000-0000-000000000000).)*$", ErrorMessage = "Please select a valid contact.")]
        public Guid ContactId { get; set; }
        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "End date is required.")]
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
        public bool IsNewRecord { get; set; } = default;
        [Required(ErrorMessage = "Label is required.")]
        public string EventLabelName { get; set; } = string.Empty;
        public bool? AllDay { get; set; }
        public List<ContactListForDropdownDTO> ContactsList { get; set; } = new() { new ContactListForDropdownDTO { ContactId = Guid.Empty, Email = "Select Contact" } };
        public List<AppointmentLabelsDTO> EventLabelList { get; set; } = new();
    }
}

