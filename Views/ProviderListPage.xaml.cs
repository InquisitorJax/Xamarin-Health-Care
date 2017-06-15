using Core;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProviderListPage : ContentPage, IView
    {
        public ProviderListPage()
        {
            InitializeComponent();
        }

        #region IView implementation

        public IViewModel ViewModel { get; set; }

        #endregion IView implementation
    }
}