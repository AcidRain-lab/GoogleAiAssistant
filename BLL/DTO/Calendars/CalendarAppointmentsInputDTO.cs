using BLL.DTO.Contacts;

namespace BLL.DTO.Calendars
{
    public class CalendarAppointmentsInputDTO
    {

        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool AllDay { get; set; }
        public CalExtendedProps ExtendedProps { get; set; } = new();
        public Guid UserId { get; set; }
        public string? Message { get; set; } = string.Empty;
        public List<string> SelectedAppointments { get; set; } = new();
    }
    public class CalExtendedProps
    {
        public string Calendar { get; set; } = "Personal";
        public List<ContactListForDropdownDTO> ContactsList { get; set; } = new();
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<Guid> Guests { get; set; } = new();
    }

}

