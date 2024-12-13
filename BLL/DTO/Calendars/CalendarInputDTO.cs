using BLL.DTO.Users;

namespace BLL.DTO.Calendars
{
    public class CalendarInputDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? Start { get; set; }
        public DateTime End { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool AllDay { get; set; }
        public CalExtendedProps ExtendedProps { get; set; } = new();
    }
    
}
