using Core;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage, IView
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.Delay(1000);

            _btnFacebook.FadeTo(1, 750, Easing.Linear);
            _btnGoogle.FadeTo(1, 750, Easing.Linear);
        }
    }
}