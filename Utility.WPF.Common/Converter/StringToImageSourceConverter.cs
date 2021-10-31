using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Utility.WPF.Common.Converter
{
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string uriString)
            {
                if (System.IO.File.Exists(uriString) == false)
                {
                    throw new Exception("dfs222 ff");
                }
                Uri imageUri = new Uri(System.IO.Path.GetFullPath(uriString), UriKind.Absolute);
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
