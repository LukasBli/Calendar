﻿using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WeeklyPlanner.UI.Converters
{
    public class ColorHexStringToSolidColorBrushConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorHex = "#00000000";
            if (value is string stringValue)
            {
                colorHex = stringValue;
            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
