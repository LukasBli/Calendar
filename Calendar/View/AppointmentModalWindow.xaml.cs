using System.Windows;

namespace WeeklyPlanner.View
{
    /// <summary>
    /// Stellt das Terminfenster dar, das zum Erstellen und Bearbeiten von Terminen verwendet wird.
    /// </summary>
    public partial class AppointmentModalWindow : Window
    {

        #region Constructors

        /// <summary>
        /// Stellt den Konstruktor dar.
        /// </summary>
        public AppointmentModalWindow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Behandelt das Klicken auf den Speichern-Button und speichert die aktuellen Daten.
        /// </summary>
        /// <param name="theSender">Das Objekt, das das Ereignis ausgelöst hat</param>
        /// <param name="theArgs">Die Ereignisdaten, die zusätzliche Informationen zum Ereignis bereitstellen.</param>
        /// <remarks>
        /// Diese Methode wird aufgerufen, wenn der Benutzer auf den Speichern-Button klickt.
        /// Sie führt die notwendigen Schritte zum Speichern der aktuellen Daten in der Anwendung aus.
        /// </remarks>
        private void ClickSave(object theSender, RoutedEventArgs theArgs)
        {
            mCreateAppointment = true;
            Close();
        }

        #endregion Events

        #region Fields

        public bool mCreateAppointment = false;
        
        #endregion Fields

    }
}
