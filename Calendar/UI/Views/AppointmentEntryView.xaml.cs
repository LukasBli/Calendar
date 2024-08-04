using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeeklyPlanner.UI.ViewModels;

namespace WeeklyPlanner.UI.Views
{
    /// <summary>
    /// Stellt den Termineintrag-<see cref="UserControl"/> auf dem Wochenplaner dar und
    /// zeigt eine Übersicht der Termininformationen an.
    /// </summary>
    public partial class AppointmentEntryView : UserControl
    {

        #region Constructors

        /// <summary>
        /// Stellt den Konstruktor dar.
        /// </summary>
        public AppointmentEntryView(AppointmentViewModel appointmentViewModel)
        {
            InitializeComponent();
            DataContext = appointmentViewModel;
        }

        #endregion Constructors

    }
}
