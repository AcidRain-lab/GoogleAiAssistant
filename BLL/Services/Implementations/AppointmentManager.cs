using BLL.Constants;
using BLL.DTO.Appointments;
using BLL.DTO.Common;
using DAL.Models;
using DAL.Repositories;
namespace BLL.Services.Implementations
{
    public class AppointmentManager : IAppointmentManager
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentManager(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<ServiceResult> DeleteAppointment(Guid id)
        {
            Appointment appointment = await _appointmentRepository.GetById(id);
            if (appointment == null)
            {
                return new ServiceResult(ServiceResultStatus.NotFound, "Appointment not found.");
            }
            int appointmentDeleted = await _appointmentRepository.Delete(appointment);
            if (appointmentDeleted > 0)
            {
                return new ServiceResult(ServiceResultStatus.Success);
            }
            return new ServiceResult(ServiceResultStatus.Error, "An error occured while deleting appointment, please try after some time.");
        }

        public async Task<ServiceResult> EditCreateAppointment(AppointmentsInputDTO input)
        {
            bool isAppointmentAlreadyScheduled = await _appointmentRepository.IsAppointmentAlreadyScheduled(input.StartDate ?? DateTime.UtcNow, input.EndDate ?? DateTime.UtcNow, input.Id);
            if (isAppointmentAlreadyScheduled)
            {
                return new ServiceResult(ServiceResultStatus.Error, ApplicationConstants.AppointmentAlreadyScheduled);
            }
            if (input.IsNewRecord)
            {
                Appointment appointment = new()
                {
                    StartDate = input.StartDate,
                    EndDate = input.EndDate ?? DateTime.UtcNow,
                    Subject = input.Subject,
                    Description = input.Description,
                    ContactId = input.ContactId,
                    UserId = input.UserId,
                    EventLabel = input.EventLabelName ?? ApplicationConstants.DefaultAppointmentLabel,
                };

                await _appointmentRepository.Save(appointment);
                return new ServiceResult(ServiceResultStatus.Success, "Appointment added successfully.");
            }
            var existingAppointment = await _appointmentRepository.GetById(input.Id);

            if (existingAppointment != null)
            {
                existingAppointment.StartDate = input.StartDate;
                existingAppointment.EndDate = input.EndDate ?? existingAppointment.EndDate;
                existingAppointment.Subject = input.Subject;
                existingAppointment.Description = input.Description;
                existingAppointment.ContactId = input.ContactId;
                existingAppointment.UserId = input.UserId;
                existingAppointment.EventLabel = input.EventLabelName ?? ApplicationConstants.DefaultAppointmentLabel;
                await _appointmentRepository.Update(existingAppointment);
                return new ServiceResult(ServiceResultStatus.Success, "Appointment updated successfully.");
            }
            return new ServiceResult(ServiceResultStatus.Error, "Appointment not created.");
        }

        public async Task<AppointmentsInputDTO> GetAppointmentById(Guid id)
        {
            var appointment = await _appointmentRepository.GetById(id);
            AppointmentsInputDTO appointmentSummary = new();
            if (appointment != null)
            {
                appointmentSummary.Id = appointment.Id;
                if (appointment.StartDate.HasValue)
                {
                    appointmentSummary.StartDate = appointment.StartDate.Value;
                }
                appointmentSummary.EndDate = appointment.EndDate;
                appointmentSummary.Subject = appointment.Subject;
                appointmentSummary.UserId = appointment.UserId;
                appointmentSummary.ContactId = appointment.ContactId;
                appointmentSummary.Description = appointment.Description ?? "";
                appointmentSummary.EventLabelName = appointment.EventLabel ?? ApplicationConstants.DefaultAppointmentLabel;
                appointmentSummary.AllDay = appointment.AllDay;
            }
            return appointmentSummary;
        }

        public async Task<List<AppointmentSummaryDTO>> GetAppointmentList()
        {
            var appointmentList = await _appointmentRepository.GetAll();
            List<AppointmentSummaryDTO> appointments = new();
            if (appointmentList.Any())
            {
                appointmentList.ForEach(appointment =>
                {
                    appointments.Add(
                    new AppointmentSummaryDTO()
                    {
                        Id = appointment.Id,
                        StartDate = appointment.StartDate,
                        EndDate = appointment.EndDate,
                        Subject = appointment.Subject,
                        ExtendedProps = new()
                        {
                            Description = appointment.Description ?? appointment.Subject,
                            Calendar = appointment.EventLabel ?? ApplicationConstants.DefaultAppointmentLabel,
                            Location = appointment.EventLocation ?? ""
                        },
                        EventUrl = appointment.EventUrl ?? "",
                        ContactId = appointment.ContactId,
                        UserId = appointment.UserId,
                        ContactName = appointment.Contact.FirstName + " " + appointment.Contact.LastName,
                        EventLabel = appointment.EventLabel,
                        AllDay = appointment.AllDay,
                    });
                });
            }
            return appointments;
        }

        public async Task<List<AppointmentSummaryDTO>> GetSelectedAppointmentList(List<string> input)
        {
            var appointmentList = await _appointmentRepository.GetSelectedAppointments(input);
            List<AppointmentSummaryDTO> appointments = new();
            if (appointmentList.Any())
            {
                appointmentList.ForEach(appointment =>
                {
                    appointments.Add(
                    new AppointmentSummaryDTO()
                    {
                        Id = appointment.Id,
                        StartDate = appointment.StartDate,
                        EndDate = appointment.EndDate,
                        Subject = appointment.Subject,
                        ExtendedProps = new()
                        {
                            Description = appointment.Description ?? "",
                            Calendar = appointment.EventLabel ?? "",
                            Location = appointment.EventLocation ?? "",
                        },
                        EventUrl = appointment.EventUrl ?? "",
                        ContactId = appointment.ContactId,
                        UserId = appointment.UserId,
                        AllDay = appointment.AllDay,
                    });
                });
            }
            return appointments;
        }
    }
}
