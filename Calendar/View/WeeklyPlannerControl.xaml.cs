using WeeklyPlanner.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WeeklyPlanner.View
{
    /// <summary>
    /// Stellt das Wochenplaner-<see cref="UserControl"/> dar, das zum Erstellen und Bearbeiten von Terminen verwendet wird.
    /// </summary>
    public partial class WeeklyPlannerControl : UserControl
    {

        #region Constructors

        /// <summary>
        /// Stellt den Konstruktor dar.
        /// </summary>
        public WeeklyPlannerControl()
        {
            DataContext = new WeeklyPlannerModel();
            InitializeComponent();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Behandelt das Klicken auf die Uhrzeitfelder und erstellt nach erfolgreicher Terminbuchung einen Terminblock.
        /// </summary>
        /// <param name="theSender">Das Objekt, das das Ereignis ausgelöst hat</param>
        /// <param name="theArgs">Die Ereignisdaten, die zusätzliche Informationen zum Ereignis bereitstellen.</param>
        private void ClickCreateAppointment(object theSender, RoutedEventArgs theArgs)
        {
            MenuItem clickedMenuItem = theSender as MenuItem;
            DateTime startTime;
            if (clickedMenuItem != null)
            {
                string time = clickedMenuItem.Tag as string + ":00";
                if (time.Count() == 4)
                    time = "0" + time;
                startTime = DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);
                AppointmentModel appointmentModel = new AppointmentModel();
                appointmentModel.StartTime = startTime;
                appointmentModel.StartDate = DateTime.Now;
                AppointmentModalWindow window = new AppointmentModalWindow();
                window.DataContext = appointmentModel;
                window.Owner = Window.GetWindow(this);
                window.ShowDialog();
                AppointmentModel model = window.DataContext as AppointmentModel;
                if (window.mCreateAppointment)
                    AddAppointmentBlock(model);
            }
        }

        /// <summary>
        /// Behandelt das Klicken auf den Zurückpfeil und wechselt zur letzten Woche.
        /// </summary>
        /// <param name="theSender">Das Objekt, das das Ereignis ausgelöst hat</param>
        /// <param name="theArgs">Die Ereignisdaten, die zusätzliche Informationen zum Ereignis bereitstellen.</param>
        private void ClickLastWeek(object theSender, RoutedEventArgs theArgs)
        {
            WeeklyPlannerModel model = DataContext as WeeklyPlannerModel;
            currentDateTime = currentDateTime.AddDays(-7);
            model.SetWeek(currentDateTime);
        }

        /// <summary>
        /// Behandelt das Klicken auf den Vorwärtspfeil und wechselt zur nächsten Woche.
        /// </summary>
        /// <param name="theSender">Das Objekt, das das Ereignis ausgelöst hat</param>
        /// <param name="theArgs">Die Ereignisdaten, die zusätzliche Informationen zum Ereignis bereitstellen.</param>
        private void ClickNextWeek(object theSender, RoutedEventArgs theArgs)
        {
            WeeklyPlannerModel model = DataContext as WeeklyPlannerModel;
            currentDateTime = currentDateTime.AddDays(7);
            model.SetWeek(currentDateTime);
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Fügt auf der Wochenplaner-Oberfläche einen Terminblock, also ein <see cref="AppointmentEntryControl"/>, hinzu.
        /// </summary>
        /// <param name="theAppointmentModel">Stellt das Modell des Termins dar.</param>
        private void AddAppointmentBlock(AppointmentModel theAppointmentModel)
        {
            AppointmentEntryControl appointmentEntry = new AppointmentEntryControl();
            appointmentEntry.DataContext = theAppointmentModel;
            interfaceGrid.Children.Add(appointmentEntry);

            int dayOfWeekColumn = theAppointmentModel.StartDate.Value.DayOfWeek != 0 ?
                                  (int)theAppointmentModel.StartDate.Value.DayOfWeek :
                                  7;

            Grid.SetColumn(appointmentEntry, dayOfWeekColumn);
        }

        #endregion Methods

        #region Fields

        private DateTime currentDateTime = DateTime.Now;

        #endregion Fields

    }
}