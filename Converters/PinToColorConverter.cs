using System;
using System.Globalization;
using Xamarin.Forms;

namespace SampleApplication.Converters
{
    public class PinToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.Transparent;
            }

            bool isPinned = (bool)value;

            var retColor = isPinned ? Color.FromHex("#DC595F") : Color.FromHex("#EE2B89");

            return retColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}