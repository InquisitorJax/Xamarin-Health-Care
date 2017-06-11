using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using SampleApplication.Droid;
using Xamarin.Forms.Platform.Android;

namespace SampleApplication
{
    [Activity(Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //TODO: replace with Forms.Context
        private static Context _appContext;

        public static Context AppContext
        {
            get { return _appContext; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            FormsAppCompatActivity.ToolbarResource = Droid.Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Droid.Resource.Layout.tabs;

            base.OnCreate(bundle);

            _appContext = this;

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App(new IocAndroidModule()));
        }
    }
}