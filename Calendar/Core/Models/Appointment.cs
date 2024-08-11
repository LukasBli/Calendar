using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;
using System.Windows.Media;

namespace WeeklyPlanner.Core.Models
{
    /// <summary>
    /// Stellt eine Datenstruktur zur Verwaltung von Tagebucheinträgen bereit.
    /// </summary>
    public class Appointment : INotifyPropertyChanged
    {

        #region Properties

        /// <summary>
        /// Ruft die Datum ab oder legt es fest.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? AppointmentDate
        {
            get
            {
                return appointmentDate;
            }
            set
            {
                appointmentDate = value;
                OnPropertyChanged(nameof(AppointmentDate));
            }
        }

        /// <summary>
        /// Ruft den Typ des Termin ab oder legt diesen fest.
        /// </summary>
        public string? AppointmentType
        {
            get
            {
                return appointmentType;
            }
            set
            {
                appointmentType = value;
                AppointmentTypeSettings();
                OnPropertyChanged(nameof(AppointmentType));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
            }
        }

        /// <summary>
        /// Ruft die Beschreibung ab oder legt sie fest.
        /// </summary>
        public string? Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        /// <summary>
        /// Ruft das Enddatum ab oder legt es fest.
        /// </summary>
        [DataType(DataType.Time)]
        public DateTime? EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                if (value != null) {
                    if (value.Value.TimeOfDay < startTime.Value.TimeOfDay)
                    {
                        endTime = startTime;
                    }
                    else if (value.Value.TimeOfDay < DateTime.Today.Add(new TimeSpan(20, 0, 0)).TimeOfDay)
                    {
                        endTime = RoundToNearestInterval(value, TimeSpan.FromMinutes(5));
                    }
                    else
                    {
                        endTime = DateTime.Today.Add(new TimeSpan(20, 0, 0));
                    }
                    OnPropertyChanged(nameof(EndTime));
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        /// <summary>
        /// Ruft die Margin eines Termineintrag ab oder legt es fest.
        /// </summary>
        [NotMapped]
        public Thickness EntryMargin
        {
            get
            {
                if (startTime.HasValue)
                {
                    int startHours = startTime.Value.Hour;
                    int startMinute = startTime.Value.Minute;

                    int newTopMargin = (startHours - 8) * 120 + (startMinute) * 2;
                    return new Thickness(0, newTopMargin, 0, 0);
                }
                return new Thickness(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Ruft die Höhe eines Termineintrag ab.
        /// </summary>
        [NotMapped]
        public double Height
        {
            get
            {
                if (StartTime.HasValue && EndTime.HasValue)
                {
                    int startHours = StartTime.Value.Hour;
                    int startMinute = StartTime.Value.Minute;
                    int endHours = EndTime.Value.Hour;
                    int endMinute = EndTime.Value.Minute;

                    int newHeight = (endHours - startHours) * 120 + (endMinute - startMinute) * 2;
                    return newHeight;
                }
                return 0;
            }
        }

        /// <summary>
        /// Ruft die Id des Termineintrag ab oder legt es fest.
        /// </summary>
        [Key]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Ruft ab oder legt fest, ob es sich um einen ganztägigen Termin handelt.
        /// </summary>
        [NotMapped]
        public bool IsFullDay
        {
            get
            {
                return startTime == DateTime.Today.Add(new TimeSpan(8, 0, 0)) &&
                       endTime == DateTime.Today.Add(new TimeSpan(20, 0, 0));
            }
            set
            {
                if (value)
                {
                    StartTime = DateTime.Today.Add(new TimeSpan(8, 0, 0));
                    EndTime = DateTime.Today.Add(new TimeSpan(20, 0, 0));
                    OnPropertyChanged(nameof(IsFullDay));
                }
            }
        }

        /// <summary>
        /// Ruft ab oder legt fest, ob der Button "Speichern" aktiviert ist.
        /// </summary>
        [NotMapped]
        public bool IsSaveButtonEnabled
        {
            get
            {
                return !string.IsNullOrEmpty(Title) &&
                       !string.IsNullOrEmpty(AppointmentType);
            }
        }

        /// <summary>
        /// Ruft den Hexadezimalcode der Seitenhintergrundfarbe ab oder legt es fest.
        /// </summary>
        public string SideBackgroundColorHex
        {
            get
            {
                return sideBackgroundColorHex;
            }
            set
            {
                sideBackgroundColorHex = value;
                OnPropertyChanged(nameof(SideBackgroundColorHex));
            }
        }

        /// <summary>
        /// Ruft das Anfangszeit ab oder legt es fest.
        /// </summary>
        [DataType(DataType.Time)]
        public DateTime? StartTime
        {
            get
            {
                return RoundToNearestInterval(startTime, TimeSpan.FromMinutes(5));
            }
            set
            {
                //TODO: Schauen das er bei vergleichen nur auf die Uhrzeit achtet
                if (endTime != null && endTime.Value.TimeOfDay < value.Value.TimeOfDay)
                {
                    startTime = endTime;
                }
                else if (value.Value.TimeOfDay > DateTime.Today.Add(new TimeSpan(8, 0, 0)).TimeOfDay)
                {
                    startTime = RoundToNearestInterval(value, TimeSpan.FromMinutes(5));
                }
                else
                {
                    startTime = DateTime.Today.Add(new TimeSpan(8, 0, 0));
                }
                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(EntryMargin));
                OnPropertyChanged(nameof(Height));
            }
        }

        /// <summary>
        /// Ruft den Titel ab oder legt diesen fest.
        /// </summary>
        [Required]
        public string? Title {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Setzt die Seitenfarbe und Endzeit entsprechend der Art des Termins.
        /// </summary>
        private void AppointmentTypeSettings()
        {
            if (appointmentType == "Arbeit")
            {
                SideBackgroundColorHex = Colors.Red.ToString();
                if (StartTime.HasValue)
                    EndTime = StartTime.Value.AddHours(2);
            }
            else if (appointmentType == "Freizeit")
            {
                SideBackgroundColorHex = Colors.Green.ToString();
                if (StartTime.HasValue)
                    EndTime = StartTime.Value.AddHours(0.5);
            }
            else if (appointmentType == "Arzttermin")
            {
                SideBackgroundColorHex = Colors.Orange.ToString();
                if (StartTime.HasValue)
                    EndTime = StartTime.Value.AddHours(1);
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
        protected void OnPropertyChanged(string thePropertyName)
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

        private DateTime? appointmentDate = DateTime.Now;
        private string? appointmentType;
        private string? description;
        private DateTime? endTime;
        private string sideBackgroundColorHex = "#00000000";
        private DateTime? startTime = DateTime.Now;
        private string? title;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion Events

    }
}
