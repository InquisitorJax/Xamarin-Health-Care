using Core.Controls;
using SampleApplication.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthProviderCard : CommandView
    {
        public static readonly BindableProperty IsForSelectionProperty = BindableProperty.Create(nameof(IsForSelection), typeof(bool), typeof(HealthProviderCard), false);
        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(HealthCareProvider), typeof(HealthProviderCard), null);

        public HealthProviderCard()
        {
            InitializeComponent();
        }

        public bool IsForSelection
        {
            get { return (bool)GetValue(IsForSelectionProperty); }
            set { SetValue(IsForSelectionProperty, value); }
        }

        public HealthCareProvider Value
        {
            get { return (HealthCareProvider)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}