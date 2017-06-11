using Core;

using Xamarin.Forms;

namespace SampleApplication.Views
{
    public partial class MainPage : MasterDetailPage, IView
    {
        public MainPage()
        {
            InitializeComponent();
        }

        #region IView implementation

        public IViewModel ViewModel { get; set; }

        #endregion IView implementation
    }
}