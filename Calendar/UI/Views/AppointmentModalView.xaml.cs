using System.Windows;
using WeeklyPlanner.UI.ViewModels;

namespace WeeklyPlanner.UI.Views
{
    /// <summary>
    /// Stellt das Terminfenster dar, das zum Erstellen und Bearbeiten von Terminen verwendet wird.
    /// </summary>
    public partial class AppointmentModalView : Window
    {

        #region Constructors

        /// <summary>
        /// Stellt den Konstruktor dar.
        /// </summary>
        public AppointmentModalView(AppointmentViewModel appointmentViewModel)
        {
            InitializeComponent();
            DataContext = appointmentViewModel;
        }

        #endregion Constructors

    }
}
