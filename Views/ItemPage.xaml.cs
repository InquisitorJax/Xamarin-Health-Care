using Core;

using Xamarin.Forms;

namespace SampleApplication.Views
{
    public partial class ItemPage : ContentPage, IView
    {
        public ItemPage()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel { get; set; }
    }
}