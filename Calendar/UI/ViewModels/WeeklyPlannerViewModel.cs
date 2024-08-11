using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Windows.Controls;
using WeeklyPlanner.Data.Repositories;
using WeeklyPlanner.UI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using WeeklyPlanner.Core.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;

namespace WeeklyPlanner.UI.ViewModels
{
    public partial class WeeklyPlannerViewModel : ObservableObject, INotifyPropertyChanged
    {

        public WeeklyPlannerViewModel(IAppointmentRepository theAppointmentRepository ,IServiceProvider theServiceProvider)
        {
            SetWeek(DateTime.Now);
            serviceProvider = theServiceProvider;
            appointmentRepository = theAppointmentRepository;
            LoadAppointments();
        }

        #region Methods

        [RelayCommand]
        private void ChangeToLastWeek()
        {
            currentDateTime = currentDateTime.AddDays(-7);
            SetWeek(currentDateTime);
            LoadAppointments();
        }

        [RelayCommand]
        private void ChangeToNextWeek()
        {
            currentDateTime = currentDateTime.AddDays(7);
            SetWeek(currentDateTime);
            LoadAppointments();
        }

        [RelayCommand]
        private void CreateAppointment()
        {
            AppointmentModalView window = serviceProvider.GetRequiredService<AppointmentModalView>();
            var appointmentViewModel = window.DataContext as AppointmentViewModel;
            appointmentViewModel.IsNew = true;
            window.ShowDialog();
            if (appointmentViewModel.SelectedAppointment != null                                       &&
                !appointmentViewModel.IsNew                                                            &&
                appointmentViewModel.SelectedAppointment.AppointmentDate >= DateTime.Parse(MondayDate) &&
                appointmentViewModel.SelectedAppointment.AppointmentDate <= DateTime.Parse(SundayDate))
            {
                AppointmentViewModels.Add(appointmentViewModel);
                OnPropertyChanged(nameof(AppointmentViewModels));
            }
        }

        // In Arbeits
        public async void LoadAppointments()
        {
            AppointmentViewModels.Clear();
            IEnumerable<Appointment> allAppointments = await appointmentRepository.GetAllAsync();
            IEnumerable<Appointment> weekAppointments = allAppointments.Where(appointment => appointment != null                                             &&
                                                                                             appointment.AppointmentDate.Value <= DateTime.Parse(SundayDate) &&
                                                                                             appointment.AppointmentDate.Value >= DateTime.Parse(MondayDate));
            IEnumerable<AppointmentViewModel> appointmentViewModels = weekAppointments.Select(a => new AppointmentViewModel(appointmentRepository, serviceProvider)
            {
                SelectedAppointment = a
            });
            AppointmentViewModels = new ObservableCollection<AppointmentViewModel>(appointmentViewModels);
            OnPropertyChanged(nameof(AppointmentViewModels));
        }

        [RelayCommand]
        private void LoadWeeklyplannerView()
        {
            var weeklyplannerView = serviceProvider.GetRequiredService<WeeklyPlannerView>();
            Grid.SetColumn(weeklyplannerView, 0);
        }

        /// <summary>
        /// Setzt die Wochentage des angegebenen Datums.
        /// </summary>
        /// <param name="theDate">Gibt das Datum an.</param>
        public void SetWeek(DateTime theDate)
        {
            WeekDays.Clear();
            for (; theDate.DayOfWeek != DayOfWeek.Monday; theDate = theDate.AddDays(-1)) ;
            for (int dayOfWeek = 1; dayOfWeek < 8; dayOfWeek++)
            {
                WeekDays.Add(theDate.ToString("dd.MM.yyyy"));
                theDate = theDate.AddDays(1);
            }

            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            int weekOfYear = cultureInfo.Calendar.GetWeekOfYear(theDate, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);
            weekOfYearText = weekOfYear.ToString() + " KW";
            OnPropertyChanged(nameof(MondayDate));
            OnPropertyChanged(nameof(TuesdayDate));
            OnPropertyChanged(nameof(WednesDate));
            OnPropertyChanged(nameof(ThursdayDate));
            OnPropertyChanged(nameof(FridayDate));
            OnPropertyChanged(nameof(SaturdayDate));
            OnPropertyChanged(nameof(SundayDate));
            OnPropertyChanged(nameof(WeekOfYearText));
        }

        #endregion Methods

        #region Properties

        public ObservableCollection<AppointmentViewModel> AppointmentViewModels
        {
            get;
            set;
        } = new ObservableCollection<AppointmentViewModel>();

        /// <summary>
        /// Ruft das Datum vom Freitag ab.
        /// </summary>
        public string FridayDate
        {
            get { return WeekDays[4]; }
        }

        /// <summary>
        /// Ruft das Datum vom Montag ab.
        /// </summary>
        public string MondayDate
        {
            get { return WeekDays[0]; }
        }

        /// <summary>
        /// Ruft das Datum vom Samstag ab.
        /// </summary>
        public string SaturdayDate
        {
            get { return WeekDays[5]; }
        }

        /// <summary>
        /// Ruft das Datum vom Sonntag ab.
        /// </summary>
        public string SundayDate
        {
            get { return WeekDays[6]; }
        }

        /// <summary>
        /// Ruft das Datum vom Donnerstag ab.
        /// </summary>
        public string ThursdayDate
        {
            get { return WeekDays[3]; }
        }

        /// <summary>
        /// Ruft das Datum vom Dienstag ab.
        /// </summary>
        public string TuesdayDate
        {
            get { return WeekDays[1]; }
        }

        /// <summary>
        /// Ruft das Datum vom Mittwoch ab.
        /// </summary>
        public string WednesDate
        {
            get { return WeekDays[2]; }
        }

        /// <summary>
        /// Ruft die Kalenderwoche ab.
        /// </summary>
        public string WeekOfYearText
        {
            get { return weekOfYearText; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Löst das <see cref="PropertyChanged"/>-Ereignis aus,
        /// um Abonnenten über Änderungen an einer Eigenschaft zu informieren.
        /// </summary>
        /// <param name="thePropertyName">
        /// Gibt den Name der Eigenschaft an, deren Wert sich geändert hat.
        /// Dieser Name wird verwendet, um die Abonnenten zu benachrichtigen, welche spezifische Eigenschaft betroffen ist.
        /// </param>
        /// <remarks>
        /// Diese Methode sollte in den Settern von Eigenschaften aufgerufen werden, wenn sich der Wert der Eigenschaft ändert.
        /// Wenn das <see cref="PropertyChanged"/>-Ereignis nicht null ist,
        /// wird es ausgelöst, um alle Abonnenten über die Änderung zu informieren.
        /// </remarks>
        protected virtual void OnPropertyChanged(string thePropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(thePropertyName));
        }

        #endregion Methods

        #region Fields

        private readonly IAppointmentRepository appointmentRepository;
        private DateTime                        currentDateTime = DateTime.Now;
        private readonly IServiceProvider       serviceProvider;
        [ObservableProperty]
        private List<string>                    weekDays        = new List<string>();
        private string                          weekOfYearText;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion Events
    }
}
