using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyPlanner.Core.Models;

namespace WeeklyPlanner.Data.Repositories
{
    public interface IAppointmentRepository
    {

        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment> GetByIdAsync(int theId);
        Task AddAsync(Appointment theAppointment);
        Task UpdateAsync(Appointment theAppointment);
        Task DeleteAsync(int theId);

    }
}
