using Core;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentSchedulePage : ContentPage, IView
    {
        public AppointmentSchedulePage()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel { get; set; }
    }
}