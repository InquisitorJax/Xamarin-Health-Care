using Core.Controls;
using SampleApplication.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthProviderCardTemp : CommandView
    {
        public static readonly BindableProperty IsForSelectionProperty = BindableProperty.Create(nameof(IsForSelection), typeof(bool), typeof(HealthProviderCard), false);
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(HealthCareProvider), typeof(HealthProviderCard), null);

        public HealthProviderCardTemp()
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