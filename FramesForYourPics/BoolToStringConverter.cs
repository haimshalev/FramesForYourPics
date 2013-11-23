using System;
using System.Globalization;
using System.Windows.Data;

namespace FramesForYourPics
{
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if ((bool) value)
                return "true";
            return "false";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}