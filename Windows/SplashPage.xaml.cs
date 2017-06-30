using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using SampleApplication.AppServices;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Core;
using Autofac;
using Core.AppServices;

namespace SampleApplication.Windows
{
    /// <summary>
    /// Extended Spash Page from
    /// https://docs.microsoft.com/en-us/windows/uwp/launch-resume/create-a-customized-splash-screen
    /// </summary>
    public sealed partial class SplashPage : Page
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        private SplashScreen splash; // Variable to hold the splash screen object.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;

        public SplashPage(SplashScreen splashscreen, bool loadState)
        {
            this.InitializeComponent();

            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This ensures that the extended splash screen formats properly in response to window resizing.
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

            splash = splashscreen;
            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);

                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();

                // If applicable, include a method for positioning a progress control.
                PositionRing();
            }

            // Create a Frame to act as the navigation context
            rootFrame = new Frame();
        }

        private void PositionRing()
        {
            splashProgressRing.SetValue(Canvas.LeftProperty, splashImageRect.X + (splashImageRect.Width * 0.5) - (splashProgressRing.Width * 0.5));
            splashProgressRing.SetValue(Canvas.TopProperty, (splashImageRect.Y + splashImageRect.Height + splashImageRect.Height * 0.1));
        }

        private void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            extendedSplashImage.Height = splashImageRect.Height;
            extendedSplashImage.Width = splashImageRect.Width;
        }

        private async void DismissedEventHandler(SplashScreen sender, object args)
        {
            dismissed = true;
            // Complete app setup operations here...
            await InitializeAsync();            
        }

        private async Task InitializeAsync()
        {
            await BootstrapperService.Initialize(new IocWindowsModule());

            GeolocationAccessStatus accessStatus = await Geolocator.RequestAccessAsync();

            //TODO: should store in settings
            var locationService = CC.IoC.Resolve<ILocationService>();
            locationService.HasGpsPermissions = accessStatus == GeolocationAccessStatus.Allowed;

            DismissExtendedSplash();
        }

        async void DismissExtendedSplash()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                rootFrame = new Frame()
                {
                    Content = new MainPage()
                };

                Window.Current.Content = rootFrame;
            });
        }

        private void ExtendedSplash_OnResize(object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be executed when a user resizes the window.
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();

                // If applicable, include a method for positioning a progress control.
                PositionRing();
            }
        }
    }
}
