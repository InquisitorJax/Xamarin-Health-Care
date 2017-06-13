// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

using SampleApplication.AppServices;
using Syncfusion.ListView.XForms.UWP;

namespace SampleApplication.Windows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            //https://developer.xamarin.com/guides/xamarin-forms/platform-features/windows/installation/universal/

            this.InitializeComponent();

            SfListViewRenderer.Init();

            BootstrapperService.Initialize(new IocWindowsModule()).GetAwaiter();

            LoadApplication(new SampleApplication.App());
        }
    }
}