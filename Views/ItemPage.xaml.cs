using Core;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemPage : ContentPage, IView
    {
        public ItemPage()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel { get; set; }
    }
}