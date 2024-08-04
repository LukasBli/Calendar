using Microsoft.EntityFrameworkCore;
using WeeklyPlanner.Core.Models;
using WeeklyPlanner.Data.Data;

namespace WeeklyPlanner.Data.Repositories
{
    internal class AppointmentRepository : IAppointmentRepository
    {

        public AppointmentRepository(ApplicationDbContext theContext)
        {
            mApplicationDbContext = theContext;
        }

        public async Task AddAsync(Appointment theAppointment)
        {
            mApplicationDbContext.Appointments.Add(theAppointment);
            await mApplicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int theId)
        {
            var appointment = await mApplicationDbContext.Appointments.FindAsync(theId);
            if (appointment != null)
            {
                mApplicationDbContext.Remove(appointment);
                await mApplicationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await mApplicationDbContext.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetByIdAsync(int theId)
        {
            return await mApplicationDbContext.Appointments.FindAsync(theId);
        }

        public async Task UpdateAsync(Appointment theAppointment)
        {
            mApplicationDbContext.Appointments.Update(theAppointment);
            await mApplicationDbContext.SaveChangesAsync();
        }

        private readonly ApplicationDbContext mApplicationDbContext;

    }
}
