using Core;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentListPage : ContentPage, IView
    {
        public AppointmentListPage()
        {
            InitializeComponent();
        }

        #region IView implementation

        public IViewModel ViewModel { get; set; }

        #endregion IView implementation
    }
}