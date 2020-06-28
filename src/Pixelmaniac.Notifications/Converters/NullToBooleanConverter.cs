using System;
using System.Globalization;
using System.Windows.Data;

namespace Pixelmaniac.Notifications.Converters
{
    internal class NullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string stringValue
                ? !string.IsNullOrEmpty(stringValue)
                : value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
