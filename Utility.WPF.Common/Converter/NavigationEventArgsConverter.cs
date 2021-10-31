using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Navigation;

namespace Utility.WPF.Common.Converter
{
    public class NavigationEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RequestNavigateEventArgs { Uri: Uri uri })
                return uri;
            throw new Exception("34r444444443");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static NavigationEventArgsConverter Instance { get; } = new NavigationEventArgsConverter();
    }
}
