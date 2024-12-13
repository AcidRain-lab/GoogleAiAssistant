using BLL.DTO.Calendars;
using BLL.DTO.Common;
namespace BLL.Services
{
    public interface ICalanderManager
    {
        Task<ServiceResult> EditCreateAdminAppointment(CalendarAppointmentsInputDTO input);
    }
}
