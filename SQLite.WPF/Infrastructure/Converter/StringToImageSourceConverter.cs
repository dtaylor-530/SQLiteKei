using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SQLite.WPF.Infrastructure.Converter
{
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string uriString)
            {
                Uri imageUri = new Uri(uriString, UriKind.Relative);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                return imageBitmap;
            }
            throw new Exception("445[v[v[f");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static StringToImageSourceConverter Instance { get; } = new StringToImageSourceConverter();
    }
}
