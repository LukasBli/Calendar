using Microsoft.EntityFrameworkCore;
using WeeklyPlanner.Core.Models;
using WeeklyPlanner.Data.Data;

namespace WeeklyPlanner.Data.Repositories
{
    internal class AppointmentRepository : IAppointmentRepository
    {

        #region Constructors

        /// <summary>
        /// Stellt den Konstruktor dar.
        /// Initialisiert eine neue Instanz des Repositories mit dem angegebenen Datenbankkontext.
        /// </summary>
        /// <param name="theContext">Der Datenbankkontext, der für die Interaktion mit den Termindaten verwendet wird.</param>
        public AppointmentRepository(ApplicationDbContext theContext)
        {
            applicationDbContext = theContext;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Fügt einen neuen Termin asynchron zur Datenbank hinzu.
        /// </summary>
        /// <param name="theAppointment">Der hinzuzufügende Termin.</param>
        /// <returns>Ein Task, der den asynchronen Vorgang repräsentiert.</returns>
        public async Task AddAsync(Appointment theAppointment)
        {
            applicationDbContext.Appointments.Add(theAppointment);
            await applicationDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Löscht einen Termin asynchron anhand seiner ID aus der Datenbank.
        /// </summary>
        /// <param name="theId">Die ID des zu löschenden Termins.</param>
        /// <returns>Ein Task, der den asynchronen Vorgang repräsentiert.</returns>
        public async Task DeleteAsync(int theId)
        {
            var appointment = await applicationDbContext.Appointments.FindAsync(theId);
            if (appointment != null)
            {
                applicationDbContext.Remove(appointment);
                await applicationDbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Ruft alle Termine asynchron aus der Datenbank ab.
        /// </summary>
        /// <returns>Ein Task, der eine Liste von Terminen repräsentiert.</returns>
        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await applicationDbContext.Appointments.ToListAsync();
        }

        /// <summary>
        /// Ruft einen Termin asynchron anhand seiner ID aus der Datenbank ab.
        /// </summary>
        /// <param name="theId">Die ID des abzurufenden Termins.</param>
        /// <returns>Ein Task, der den abgerufenen Termin repräsentiert.</returns>
        public async Task<Appointment> GetByIdAsync(int theId)
        {
            return await applicationDbContext.Appointments.FindAsync(theId);
        }

        /// <summary>
        /// Aktualisiert einen bestehenden Termin asynchron in der Datenbank.
        /// </summary>
        /// <param name="theAppointment">Der zu aktualisierende Termin.</param>
        /// <returns>Ein Task, der den asynchronen Vorgang repräsentiert.</returns>
        public async Task UpdateAsync(Appointment theAppointment)
        {
            applicationDbContext.Appointments.Update(theAppointment);
            await applicationDbContext.SaveChangesAsync();
        }

        #endregion Methods

        #region Fields

        private readonly ApplicationDbContext applicationDbContext;

        #endregion Fields

    }
}
