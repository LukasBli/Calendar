using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace WeeklyPlanner.Model
{
    /// <summary>
    /// Stellt eine Datenstruktur zur Verwaltung von des Wochenplanners bereit.
    /// </summary>
    public class WeeklyPlannerModel : INotifyPropertyChanged
    {

        #region Constructors

        /// <summary>
        /// Stellt den Konstruktor dar.
        /// </summary>
        public WeeklyPlannerModel()
        {
            SetWeek(DateTime.Now);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Ruft das Datum vom Freitag ab.
        /// </summary>
        public string FridayDate
        {
            get { return mWeekDays[4]; }
        }

        /// <summary>
        /// Ruft das Datum vom Montag ab.
        /// </summary>
        public string MondayDate
        {
            get { return mWeekDays[0]; }
        }

        /// <summary>
        /// Ruft das Datum vom Samstag ab.
        /// </summary>
        public string SaturdayDate
        {
            get { return mWeekDays[5]; }
        }

        /// <summary>
        /// Ruft das Datum vom Sonntag ab.
        /// </summary>
        public string SundayDate
        {
            get { return mWeekDays[6]; }
        }

        /// <summary>
        /// Ruft das Datum vom Donnerstag ab.
        /// </summary>
        public string ThursdayDate
        {
            get { return mWeekDays[3]; }
        }

        /// <summary>
        /// Ruft das Datum vom Dienstag ab.
        /// </summary>
        public string TuesdayDate
        {
            get { return mWeekDays[1]; }
        }

        /// <summary>
        /// Ruft das Datum vom Mittwoch ab.
        /// </summary>
        public string WednesDate
        {
            get { return mWeekDays[2]; }
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

        /// <summary>
        /// Setzt die Wochentage des angegebenen Datums.
        /// </summary>
        /// <param name="theDate">Gibt das Datum an.</param>
        public void SetWeek(DateTime theDate)
        {
            mWeekDays.Clear();
            for (; theDate.DayOfWeek != DayOfWeek.Monday; theDate = theDate.AddDays(-1)) ;
            for (int dayOfWeek = 1; dayOfWeek < 8; dayOfWeek++)
            {
                mWeekDays.Add(theDate.ToString("dd.MM.yyyy"));
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

        #region Fields

        private List<string>    mWeekDays       = new List<string>();
        private string          mWeekOfYearText;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events
    }
}
