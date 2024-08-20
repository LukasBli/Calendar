using System.Globalization;
using System.Windows.Data;

namespace WeeklyPlanner.UI.Converters
{
    class DateTimeToHHmmStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTimeValue) {
                return dateTimeValue.ToString("HH:mm");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
