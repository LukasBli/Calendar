using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WeeklyPlanner.Core.Models;
using WeeklyPlanner.Data.Data;
using WeeklyPlanner.Data.Repositories;
using WeeklyPlanner.UI.Views;

namespace WeeklyPlanner.UI.ViewModels
{
    public partial class AppointmentViewModel : ObservableObject
    {
        private readonly IAppointmentRepository mAppointmentRepository;
        private readonly IServiceProvider mServiceProvider;
        private Appointment selectedAppointment = new Appointment();

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

        [ObservableProperty]
        private bool isNew = false;

        public AppointmentViewModel(IAppointmentRepository theAppointmentRepository, IServiceProvider serviceProvider)
        {
            mAppointmentRepository = theAppointmentRepository;
            mServiceProvider = serviceProvider;
        }

        [RelayCommand]
        private async Task SaveAppointment(AppointmentModalView window)
        {

            if (SelectedAppointment != null)
            {
                if (IsNew)
                {
                    await mAppointmentRepository.AddAsync(SelectedAppointment);
                }
                else
                {
                    await mAppointmentRepository.UpdateAsync(SelectedAppointment);
                }
                AppointmentEntryView appointmentEntryView = mServiceProvider.GetRequiredService<AppointmentEntryView>();
                appointmentEntryView.DataContext = window.DataContext;
            }
            window.Close();
        }

        [RelayCommand]
        private async Task OpenAppointment()
        {
            AppointmentModalView window = mServiceProvider.GetRequiredService<AppointmentModalView>();
            var dataContext = window.DataContext as AppointmentViewModel;
            dataContext.SelectedAppointment = SelectedAppointment;
            window.ShowDialog();
        }
    }
}
