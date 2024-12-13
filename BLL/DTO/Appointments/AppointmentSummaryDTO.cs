using BLL.DTO.Calendars;

namespace BLL.DTO.Appointments
{
    public class AppointmentSummaryDTO
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? ContactId { get; set; }
        public Guid? UserId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string EventUrl { get; set; } = string.Empty;
        public string? ContactName { get; set; } = string.Empty;
        public string? EventLabel { get; set; } = string.Empty;
        public bool? AllDay { get; set; }
        public CalExtendedProps ExtendedProps { get; set; } = new();
    }
}
