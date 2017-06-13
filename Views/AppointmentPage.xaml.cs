using Core;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentPage : ContentPage, IView
    {
        public AppointmentPage()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel { get; set; }
    }
}