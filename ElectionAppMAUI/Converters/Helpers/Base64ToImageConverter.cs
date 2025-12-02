using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ElectionAppMAUI.Converters
{
    public class Base64ToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string base64 && base64.Contains("base64,"))
            {
                try
                {
                    var clean = base64.Substring(base64.IndexOf(",") + 1);
                    var bytes = System.Convert.FromBase64String(clean);
                    return ImageSource.FromStream(() => new MemoryStream(bytes));
                }
                catch
                {
                    return null;
                }
            }

            return value; // повертаємо як URL
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
