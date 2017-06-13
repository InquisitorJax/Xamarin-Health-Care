using Core.Controls;
using SampleApplication.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthProviderCard : CommandView
    {
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(Appointment), typeof(HealthCareProvider), null);

        public HealthProviderCard()
        {
            InitializeComponent();
        }

        public HealthCareProvider Value
        {
            get { return (HealthCareProvider)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}