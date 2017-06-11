using System;
using System.IO;
using Xamarin.Forms;

namespace Core
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                byte[] bytes = (byte[])value;
                ImageSource retImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                return retImageSource;
            }

            if (parameter != null)
            {
                string fillerIcon = (string)parameter;
                ImageSource retImageSource = ImageSource.FromFile(fillerIcon);
                return retImageSource;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IValueConverter implementation
    }
}