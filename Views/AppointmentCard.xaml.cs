using Core.Controls;
using SampleApplication.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentCard : CommandView
    {
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(Appointment), typeof(AppointmentCard), null);

        public AppointmentCard()
        {
            InitializeComponent();
        }

        public Appointment Value
        {
            get { return (Appointment)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}