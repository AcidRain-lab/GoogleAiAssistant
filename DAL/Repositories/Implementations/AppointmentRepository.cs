using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly CrmContext _context;
        public AppointmentRepository(CrmContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAll()
        {
            var appointments = await _context.Appointments.Include(x => x.Contact).ToListAsync().ConfigureAwait(false);
            return appointments;
        }

        public async Task<Appointment> GetById(Guid id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            return appointment!;
        }

        public async Task<int> Save(Appointment appointment)
        {

            await _context.AddAsync(appointment);
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> Update(Appointment appointment)
        {
            _context.Update(appointment);
            return await _context.SaveChangesAsync();
        }
        public async Task<bool> IsAppointmentAlreadyScheduled(DateTime startDate, DateTime endDate, Guid id)
        {
            return await _context.Appointments.AnyAsync(a =>
                                             a.Id != id &&
                                             startDate < a.EndDate &&
                                             endDate > a.StartDate);
        }

        public async Task<List<Appointment>> GetSelectedAppointments(List<string> input)
        {
            var appointments = await _context.Appointments.Where(appointment => input.Contains(appointment.EventLabel ?? string.Empty)).ToListAsync().ConfigureAwait(false);
            return appointments;
        }
    }
}
