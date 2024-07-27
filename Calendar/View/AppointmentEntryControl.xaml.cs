using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WeeklyPlanner.View
{
    /// <summary>
    /// Stellt den Termineintrag-<see cref="UserControl"/> auf dem Wochenplaner dar und
    /// zeigt eine Übersicht der Termininformationen an.
    /// </summary>
    public partial class AppointmentEntryControl : UserControl
    {

        #region Constructors

        /// <summary>
        /// Stellt den Konstruktor dar.
        /// </summary>
        public AppointmentEntryControl()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Behandelt das Doppelklicken auf dieses <see cref="UserControl"/> und
        /// öffnet anschließend das <see cref="AppointmentModalWindow"/> mit den Details des Eintrags.
        /// </summary>
        /// <param name="theSender">Das Objekt, das das Ereignis ausgelöst hat</param>
        /// <param name="theArgs">Die Ereignisdaten, die zusätzliche Informationen zum Ereignis bereitstellen.</param>
        private void OpenDetailWithMouseLeftButtonDown(object theSender, MouseButtonEventArgs theArgs)
        {
            if (theArgs.ClickCount == 2)
            {
                Border border = theSender as Border;
                if (border != null)
                {
                    AppointmentModalWindow window = new AppointmentModalWindow();
                    window.DataContext = border.DataContext;
                    window.Owner = Window.GetWindow(this);
                    window.ShowDialog();
                }
            }
        }

        #endregion Events

    }
}
