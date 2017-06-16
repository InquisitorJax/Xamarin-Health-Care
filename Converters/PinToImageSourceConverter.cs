using System;
using System.Globalization;
using Xamarin.Forms;

namespace SampleApplication.Converters
{
    public class PinToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            bool isPinned = (bool)value;

            ImageSource pinImage = ImageSource.FromFile("pin_dark");
            ImageSource unpinImage = ImageSource.FromFile("unpin_dark");

            var retColor = isPinned ? unpinImage : pinImage;

            return retColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}