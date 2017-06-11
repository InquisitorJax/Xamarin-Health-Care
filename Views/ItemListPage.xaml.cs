using Core;

using Xamarin.Forms;

namespace SampleApplication.Views
{
    public partial class ItemListPage : ContentPage, IView
    {
        public ItemListPage()
        {
            InitializeComponent();
        }

        #region IView implementation

        public IViewModel ViewModel { get; set; }

        #endregion IView implementation
    }
}