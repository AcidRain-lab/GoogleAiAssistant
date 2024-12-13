
using BLL.Constants;
using BLL.DTO.Calendars;
using BLL.DTO.Common;
using BLL.Enums;
using DAL.Models;
using DAL.Repositories;
namespace BLL.Services.Implementations
{
    public class CalanderManager : ICalanderManager
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public CalanderManager(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        public async Task<ServiceResult> EditCreateAdminAppointment(CalendarAppointmentsInputDTO input)
        {
            if (input == null)
            {
                return new ServiceResult(ServiceResultStatus.Error, "No record to save or update.");
            }
            
            bool isAppointmentAlreadyScheduled = await _appointmentRepository.IsAppointmentAlreadyScheduled(input.Start, input.End, input.Id);
            if (isAppointmentAlreadyScheduled)
            {
                return new ServiceResult(ServiceResultStatus.Error, ApplicationConstants.AppointmentAlreadyScheduled);
            }
            if (input.Id == Guid.Empty)
            {

                Appointment appointment = new()
                {
                    Subject = input.Title,
                    StartDate = input.Start,
                    EndDate = input.End,
                    UserId = input.UserId,
                    ContactId = input.ExtendedProps.Guests.FirstOrDefault(),
                    AllDay = input.AllDay,
                    Description = input.ExtendedProps.Description,
                    EventLabel = input.ExtendedProps.Calendar,
                    IsActive = true,
                    EventStatus = (int)AppointmentStatusEnum.None,
                    EventLocation = input.ExtendedProps.Location,
                    EventUrl = input.Url,
                };
                await _appointmentRepository.Save(appointment);
                return new ServiceResult(ServiceResultStatus.Success, "Appointment created successfully.");
            }
            Appointment existingAppointment = await _appointmentRepository.GetById(input.Id);
            if (existingAppointment != null)
            {
                existingAppointment.Subject = input.Title;
                existingAppointment.StartDate = input.Start;
                existingAppointment.EndDate = input.End;
                existingAppointment.UserId = input.UserId;
                existingAppointment.ContactId = input.ExtendedProps.Guests.FirstOrDefault();
                existingAppointment.AllDay = input.AllDay;
                existingAppointment.Description = input.ExtendedProps.Description;
                existingAppointment.EventLabel = input.ExtendedProps.Calendar;
                existingAppointment.IsActive = true;
                existingAppointment.EventStatus = (int)AppointmentStatusEnum.None;
                existingAppointment.EventLocation = input.ExtendedProps.Location;
                existingAppointment.EventUrl = input.Url;
                existingAppointment.ModifiedAt = DateTime.UtcNow;
                existingAppointment.ModifiedBy = input.UserId;
                await _appointmentRepository.Update(existingAppointment);
                return new ServiceResult(ServiceResultStatus.Success, "Appointment updated successfully.");
            }
            return new ServiceResult(ServiceResultStatus.Error, "Appointment not scheduled, please try after some time.");
        }
    }
}
