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

        public AppointmentViewModel(IAppointmentRepository theAppointmentRepository, IServiceProvider theServiceProvider)
        {
            appointmentRepository = theAppointmentRepository;
            serviceProvider = theServiceProvider;
        }

        #region Properties

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
