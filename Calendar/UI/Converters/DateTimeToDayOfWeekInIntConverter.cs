using System.Globalization;
using System.Windows.Data;

namespace WeeklyPlanner.UI.Converters
{
    public class DateTimeToDayOfWeekInIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int dayOfWeekColumn = 1;
            if (value is DateTime dateTimeValue)
            {
                dayOfWeekColumn = dateTimeValue.DayOfWeek != 0 ?
                                  (int)dateTimeValue.DayOfWeek :
                                  7;
            }
            return dayOfWeekColumn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
