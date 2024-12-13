using DAL.Models;

namespace DAL.Repositories
{
    public interface IAppointmentRepository
    {
        Task<int> Update(Appointment appointment);
        Task<int> Delete(Appointment appointment);
        Task<int> Save(Appointment appointment);
        Task<Appointment> GetById(Guid id);
        Task<List<Appointment>> GetAll();
        Task<List<Appointment>> GetSelectedAppointments(List<string> input);
        Task<bool> IsAppointmentAlreadyScheduled(DateTime startDate, DateTime endDate, Guid id);
    }
}
