using System;
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
        [Key]
        public int Id
        {
            get;
            set;
        }

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

        [DataType(DataType.Time)]
        public DateTime? EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                if (value < startTime)
                {
                    endTime = startTime;
                }
                else if (value < DateTime.Today.Add(new TimeSpan(20, 0, 0)))
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

        /// <summary>
        /// Ruft die Margin eines Eintrags ab oder legt sie fest.
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
        /// Ruft die Höhe eines Eintrags ab.
        /// </summary>
        [NotMapped]
        public double Height
        {
            get
            {
                if (startTime.HasValue && endTime.HasValue)
                {
                    int startHours = startTime.Value.Hour;
                    int startMinute = startTime.Value.Minute;
                    int endHours = endTime.Value.Hour;
                    int endMinute = endTime.Value.Minute;

                    int newHeight = (endHours - startHours) * 120 + (endMinute - startMinute) * 2;
                    return newHeight;
                }
                return 0;
            }
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
        /// Ruft die Seitlichefarbe ab oder legt sie fest.
        /// </summary>
        [NotMapped]
        public Color SideBackgroundColor
        {
            get
            {
                return sideBackgroundColor;
            }
            set
            {
                sideBackgroundColor = value;
                OnPropertyChanged(nameof(SideBackgroundColor));
                OnPropertyChanged(nameof(SideBackgroundBrush));
            }
        }

        // TODO: Dafür einen Converter schreiben
        public SolidColorBrush SideBackgroundBrush
        {
            get
            {
                SolidColorBrush sideBackgroundBrush = new SolidColorBrush(sideBackgroundColor);
                return sideBackgroundBrush;
            }
        }

        [DataType(DataType.Time)]
        public DateTime? StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                if (endTime < value)
                {
                    startTime = endTime;
                }
                else if (value > DateTime.Today.Add(new TimeSpan(8, 0, 0)))
                {
                    startTime = RoundToNearestInterval(value, TimeSpan.FromMinutes(5));
                }
                else
                {
                    startTime = DateTime.Today.Add(new TimeSpan(8, 0, 0));
                }
                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(StartTimeString));
                OnPropertyChanged(nameof(EntryMargin));
                OnPropertyChanged(nameof(Height));
            }
        }

        // TODO: Dafür einen Converter schreiben
        [NotMapped]
        public string StartTimeString
        {
            get
            {
                return StartTime.Value.ToString("HH:mm");
            }
        }

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

        private string? description;
        private DateTime? appointmentDate = DateTime.Now;
        private string? appointmentType;
        private DateTime? endTime;
        private Color sideBackgroundColor;
        private DateTime? startTime = DateTime.Now;
        private string? title;

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

        /// <summary>
        /// Setzt die Seitenfarbe und Endzeit entsprechend der Art des Termins.
        /// </summary>
        private void AppointmentTypeSettings()
        {
            if (appointmentType == "Arbeit")
            {
                sideBackgroundColor = Colors.Red;
                if (StartTime.HasValue)
                EndTime = StartTime.Value.AddHours(8.5);
            }
            else if (appointmentType == "Freizeit")
            {
                SideBackgroundColor = Colors.Green;
                if (StartTime.HasValue)
                EndTime = StartTime.Value.AddHours(0.5);
            }
            else if (appointmentType == "Termin")
            {
                SideBackgroundColor = Colors.Orange;
                if (StartTime.HasValue)
                EndTime = StartTime.Value.AddHours(1);
            }
            OnPropertyChanged(nameof(EndTime));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
