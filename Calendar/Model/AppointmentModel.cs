using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace WeeklyPlanner.Model
{
    /// <summary>
    /// Stellt eine Datenstruktur zur Verwaltung von Tagebucheinträgen bereit.
    /// </summary>
    public class AppointmentModel : INotifyPropertyChanged
    {

        #region Properties

        /// <summary>
        /// Ruft die Beschreibung ab oder legt sie fest.
        /// </summary>
        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        /// <summary>
        /// Ruft den Typ des Termin ab oder legt diesen fest.
        /// </summary>
        public string AppointmentType
        {
            get
            {
                return mAppointmentType;
            }
            set
            {
                mAppointmentType = value;
                AppointmentTypeSettings();
                OnPropertyChanged(nameof(AppointmentType));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
                OnPropertyChanged(nameof(SideBackgroundColor));
                OnPropertyChanged(nameof(SideBackgroundBrush));
                OnPropertyChanged(nameof(EndTime));
                OnPropertyChanged(nameof(Height));
            }
        }

        /// <summary>
        /// Ruft das Enddatum ab oder legt es fest.
        /// </summary>
        public DateTime? EndTime
        {
            get
            {
                return mEndTime;
            }
            set
            {
                if (value < mStartTime)
                {
                    mEndTime = mStartTime;
                }
                else if (value < DateTime.Today.Add(new TimeSpan(20, 0, 0)))
                {
                    mEndTime = RoundToNearestInterval(value, TimeSpan.FromMinutes(5));
                }
                else
                {
                    mEndTime = DateTime.Today.Add(new TimeSpan(20, 0, 0));
                }

                OnPropertyChanged(nameof(EndTime));
                OnPropertyChanged(nameof(IsFullDay));
                OnPropertyChanged(nameof(Height));
            }
        }

        /// <summary>
        /// Ruft die Margin eines Eintrags ab oder legt sie fest.
        /// </summary>
        public Thickness EntryMargin
        {
            get
            {
                if (mStartTime.HasValue)
                {
                    int startHours = mStartTime.Value.Hour;
                    int startMinute = mStartTime.Value.Minute;

                    int newTopMargin = (startHours - 8) * 120 + (startMinute) * 2;
                    return new Thickness(0, newTopMargin, 0, 0);
                }
                return new Thickness(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Ruft die Höhe eines Eintrags ab.
        /// </summary>
        public double Height
        {
            get
            {
                if (mStartTime.HasValue && mEndTime.HasValue)
                {
                    int startHours = mStartTime.Value.Hour;
                    int startMinute = mStartTime.Value.Minute;
                    int endHours = mEndTime.Value.Hour;
                    int endMinute = mEndTime.Value.Minute;

                    int newHeight = (endHours - startHours) * 120 + (endMinute - startMinute) * 2;
                    return newHeight;
                }
                return 0;
            }
        }

        /// <summary>
        /// Ruft ab oder legt fest, ob es sich um einen ganztägigen Termin handelt.
        /// </summary>
        public bool IsFullDay
        {
            get
            {
                return mStartTime == DateTime.Today.Add(new TimeSpan(8,0,0)) &&
                       mEndTime == DateTime.Today.Add(new TimeSpan(20,0,0));
            }
            set
            {
                if (value)
                {
                    StartTime = DateTime.Today.Add(new TimeSpan(8,0,0));
                    EndTime = DateTime.Today.Add(new TimeSpan(20,0,0));
                    OnPropertyChanged(nameof(IsFullDay));
                }
            }
        }

        /// <summary>
        /// Ruft ab oder legt fest, ob der Button "Speichern" aktiviert ist.
        /// </summary>
        public bool IsSaveButtonEnabled
        {
            get
            {
                return ! string.IsNullOrEmpty(Title) &&
                       ! string.IsNullOrEmpty(AppointmentType);
            }
        }

        /// <summary>
        /// Ruft die Seitlichefarbe ab oder legt sie fest.
        /// </summary>
        public Color SideBackgroundColor
        {
            get
            {
                return mSideBackgroundColor;
            }
            set
            {
                mSideBackgroundColor = value;
                OnPropertyChanged(nameof(SideBackgroundColor));
                OnPropertyChanged(nameof(SideBackgroundBrush));
            }
        }

        // TODO: Dafür einen Converter schreiben
        public SolidColorBrush SideBackgroundBrush
        {
            get
            {
                SolidColorBrush sideBackgroundBrush = new SolidColorBrush(mSideBackgroundColor);
                return sideBackgroundBrush;
            }
        }

        /// <summary>
        /// Ruft die Anfangsdatum ab oder legt es fest.
        /// </summary>
        public DateTime? StartDate
        {
            get
            {
                return mStartDate;
            }
            set
            {
                mStartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        /// <summary>
        /// Ruft das Anfangszeit ab oder legt es fest.
        /// </summary>
        public DateTime? StartTime
        {
            get
            {
                return mStartTime;
            }
            set
            {
                if (mEndTime < value) {
                    mStartTime = mEndTime;
                } else if (value > DateTime.Today.Add(new TimeSpan(8, 0, 0))) {
                    mStartTime = RoundToNearestInterval(value, TimeSpan.FromMinutes(5));
                } else {
                    mStartTime = DateTime.Today.Add(new TimeSpan(8, 0, 0));
                }

                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(IsFullDay));
                OnPropertyChanged(nameof(StartTimeString));
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(EntryMargin));
            }
        }

        // TODO: Dafür einen Converter schreiben
        public string StartTimeString
        {
            get
            {
                return mStartTime.Value.ToString("HH:mm");
            }
        }

        /// <summary>
        /// Ruft den Titel ab oder legt diesen fest.
        /// </summary>
        public string Title
        {
            get
            {
                return mTitle;
            }
            set
            {
                mTitle = value;
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
                OnPropertyChanged(nameof(Title));
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Setzt die Seitenfarbe und Endzeit entsprechend der Art des Termins.
        /// </summary>
        private void AppointmentTypeSettings()
        {
            if (mAppointmentType == "Arbeit")
            {
                mSideBackgroundColor = Colors.Red;
                if (mStartTime.HasValue)
                    mEndTime = mStartTime.Value.AddHours(8.5);
            }
            else if (mAppointmentType == "Freizeit")
            {
                mSideBackgroundColor = Colors.Green;
                if (mStartTime.HasValue)
                    mEndTime = mStartTime.Value.AddHours(0.5);
            }
            else if (mAppointmentType == "Termin")
            {
                mSideBackgroundColor = Colors.Orange;
                if (mStartTime.HasValue)
                    mEndTime = mStartTime.Value.AddHours(1);
            }
        }

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
        /// Rundet die angegebenen Datum-Uhrzeit auf das nächstgelegene Intervall auf oder ab.
        /// </summary>
        /// <param name="theDateTime">
        /// Gibt die Datum-Uhrzeit an, die gerundet werden soll.
        /// Wenn dieser Wert null ist, wird das aktuelle Datum und die aktuelle Uhrzeit zurückgegeben.
        /// </param>
        /// <param name="theInterval">
        /// Gibt das Intervall an, auf das der Datum-Uhrzeit-Wert gerundet werden soll.
        /// </param>
        /// <returns>
        /// Die gerundete Datum-Uhrzeit gemäß dem angegebenen Intervall.
        /// </returns>
        private DateTime RoundToNearestInterval(DateTime? theDateTime, TimeSpan theInterval)
        {
            if (theDateTime.HasValue)
            {
                long ticks = (theDateTime.Value.Ticks + theInterval.Ticks / 2) / theInterval.Ticks * theInterval.Ticks;
                return new DateTime(ticks);
            }
            return DateTime.Now;
        }

        #endregion Methods

        #region Fields

        private string      mAppointmentType;
        private string      mDescription;
        private DateTime?   mEndTime = DateTime.Now < DateTime.Today.Add(new TimeSpan(20, 0, 0)) ?
                                DateTime.Today.Add(new TimeSpan(DateTime.Now.Hour, 0, 0)) :
                                DateTime.Today.Add(new TimeSpan(20, 0, 0));
        private bool        mIsSaveButtonEnabled = false;
        private Color       mSideBackgroundColor;
        private DateTime?   mStartDate;
        private DateTime?   mStartTime;
        private string      mTitle;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

    }
}
