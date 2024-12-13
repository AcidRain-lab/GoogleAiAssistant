

using BLL.DTO.Appointments;
using BLL.DTO.Common;

namespace BLL.Services
{
    public interface IAppointmentManager
    {
        Task<ServiceResult> EditCreateAppointment(AppointmentsInputDTO input);
        Task<List<AppointmentSummaryDTO>> GetAppointmentList();
        Task<AppointmentsInputDTO> GetAppointmentById(Guid id);
        Task<ServiceResult> DeleteAppointment(Guid id);
        Task<List<AppointmentSummaryDTO>> GetSelectedAppointmentList(List<string> input);
    }
}
