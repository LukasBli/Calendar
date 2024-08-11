using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WeeklyPlanner.UI.Converters
{
    public class ColorHexStringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorHex = "#00000000";
            if (value is string stringValue)
            {
                colorHex = stringValue;
            }
            return (Color)ColorConverter.ConvertFromString(colorHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
