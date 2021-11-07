using System;
using System.Globalization;
using System.IO;
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
                var file = new FileInfo(uriString);
                if (file.Exists == false)
                {
                    throw new Exception($"file, {file.FullName}, does not exist");
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
