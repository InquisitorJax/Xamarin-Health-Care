using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityIndicatorMessage : ContentView
    {
        public ActivityIndicatorMessage()
        {
            InitializeComponent();
            //BUG: ECG isn't animating
            //_busyIndicator.AnimationType = Syncfusion.SfBusyIndicator.XForms.AnimationTypes.ECG;
        }
    }
}