using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using WeeklyPlanner.Core.Models;
using WeeklyPlanner.Data.Repositories;
using WeeklyPlanner.UI.Views;

namespace WeeklyPlanner.UI.ViewModels
{
    public partial class AppointmentViewModel : ObservableObject
    {

        #region Constructors

        /// <summary>
        /// Stellt den Konstruktor dar.
        /// Initialisiert eine neue Instanz des ViewModels mit dem angegebenen Repository und dem ServiceProvider.
        /// </summary>
        /// <param name="theAppointmentRepository">Das Repository für die Verwaltung von Terminen.</param>
        /// <param name="theServiceProvider">Der ServiceProvider, der für die Bereitstellung von Diensten verwendet wird.</param>
        public AppointmentViewModel(IAppointmentRepository theAppointmentRepository, IServiceProvider theServiceProvider)
        {
            appointmentRepository = theAppointmentRepository;
            serviceProvider = theServiceProvider;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Ruft den ausgewählten Termin ab oder legt ihn fest.
        /// </summary>
        public Appointment SelectedAppointment
        {
            get
            {
                return selectedAppointment;
            }
            set
            {
                selectedAppointment = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Öffnet das Fenster zur Bearbeitung des ausgewählten Termins.
        /// </summary>
        [RelayCommand]
        private void OpenAppointment()
        {
            AppointmentModalView window = serviceProvider.GetRequiredService<AppointmentModalView>();
            var dataContext = window.DataContext as AppointmentViewModel;
            if (dataContext != null)
            {
                dataContext.SelectedAppointment = SelectedAppointment;
                window.ShowDialog();
            }
        }

        /// <summary>
        /// Speichert den aktuellen Termin. Falls der Termin neu ist, wird er hinzugefügt, andernfalls wird er aktualisiert.
        /// </summary>
        /// <param name="window">Das Fenster, das nach dem Speichern geschlossen wird.</param>
        /// <returns>Ein Task, der den asynchronen Speichervorgang repräsentiert.</returns>
        [RelayCommand]
        private async Task SaveAppointment(AppointmentModalView window)
        {
            if (SelectedAppointment != null)
            {
                if (IsNew)
                {
                    await appointmentRepository.AddAsync(SelectedAppointment);
                    IsNew = false;
                }
                else
                {
                    await appointmentRepository.UpdateAsync(SelectedAppointment);
                }
            }
            window.Close();
        }

        #endregion Methods

        #region Fields

        private readonly IAppointmentRepository appointmentRepository;
        [ObservableProperty]
        private bool isNew = false;
        private Appointment selectedAppointment = new Appointment();
        private readonly IServiceProvider serviceProvider;

        #endregion Fields

    }
}
