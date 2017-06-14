using System;
using System.Globalization;
using Xamarin.Forms;

namespace Core.Converters
{
    public class NullValueToFillTextConverter : IValueConverter
    {
        private const string Text = "<Not Specified>";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fillText = parameter == null ? Text : parameter.ToString();

            string retValue = value == null ? fillText : value.ToString();

            if (string.IsNullOrWhiteSpace(retValue))
            {
                retValue = fillText;
            }

            return retValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}