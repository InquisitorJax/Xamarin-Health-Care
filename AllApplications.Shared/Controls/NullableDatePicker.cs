using System;
using Xamarin.Forms;

namespace Core.Controls
{
    public class NullableDatePicker : DatePicker
    {
        //TEMP until xamarin supports nullable date
        //FROM: https://forums.xamarin.com/discussion/20028/datepicker-possible-to-bind-to-nullable-date-value

        public static readonly BindableProperty NullableDateProperty = BindableProperty.Create("NullableDate", typeof(DateTime?), typeof(NullableDatePicker), DateTime.Now);
        private string _format = null;

        public DateTime? NullableDate
        {
            get { return (DateTime?)GetValue(NullableDateProperty); }
            set { SetValue(NullableDateProperty, value); UpdateDate(); }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateDate();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Date") NullableDate = Date;
        }

        private void UpdateDate()
        {
            if (NullableDate.HasValue) { if (null != _format) Format = _format; Date = NullableDate.Value; }
            else { _format = Format; Format = "pick ..."; }
        }
    }
}