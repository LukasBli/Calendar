using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Windows.Controls;
using System.Windows;
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
        private readonly IServiceProvider mServiceProvider;

        public ObservableCollection<AppointmentViewModel> AppointmentViewModels { get; set; } = new ObservableCollection<AppointmentViewModel>();

        public WeeklyPlannerViewModel(IServiceProvider serviceProvider)
        {
            SetWeek(DateTime.Now);
            mServiceProvider = serviceProvider;
        }

        [RelayCommand]
        private void LoadData()
        {
            // Implement your logic here
        }

        [RelayCommand]
        private void CreateAppointment()
        {
            AppointmentModalView window = mServiceProvider.GetRequiredService<AppointmentModalView>();
            var appointmentViewModel = window.DataContext as AppointmentViewModel;
            appointmentViewModel.IsNew = true;
            window.ShowDialog();
            AppointmentEntryView appointmentEntryView = mServiceProvider.GetRequiredService<AppointmentEntryView>();
            Grid.SetColumn(appointmentEntryView, 1);

            AppointmentViewModels.Add(appointmentViewModel);
            OnPropertyChanged(nameof(AppointmentViewModels));
        }

        [RelayCommand]
        private void LoadWeeklyplannerView()
        {
            var weeklyplannerView = mServiceProvider.GetRequiredService<WeeklyPlannerView>();
            Grid.SetColumn(weeklyplannerView, 0);
        }

        #region Events

        [RelayCommand]
        private void ChangeToLastWeek()
        {
            currentDateTime = currentDateTime.AddDays(-7);
            SetWeek(currentDateTime);
        }

        [RelayCommand]
        private void ChangeToNextWeek()
        {
            currentDateTime = currentDateTime.AddDays(7);
            SetWeek(currentDateTime);
        }

        #endregion Events

        #region Methods

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
            mWeekOfYearText = weekOfYear.ToString() + " KW";
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
            get { return mWeekOfYearText; }
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

        [ObservableProperty]
        private List<string> weekDays = new List<string>();

        private string mWeekOfYearText;

        private DateTime currentDateTime = DateTime.Now;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events
    }
}
