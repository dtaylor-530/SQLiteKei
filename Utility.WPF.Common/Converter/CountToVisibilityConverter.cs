using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Utility.WPF.Common.Converter
{
    public class CountToVisibilityConverter : IValueConverter
    {
        public bool? Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var inverse = Invert ?? (bool?)parameter ?? false;
            if (value is not int count)
                throw new Exception("rg4343");
            if (count > 0 != inverse)
            {
                return Visibility.Visible;
            }
            else return Visibility.Hidden;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static CountToVisibilityConverter Instance { get; } = new();
    }
}
