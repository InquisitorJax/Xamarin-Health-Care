using Autofac;
using Core;
using Core.AppServices;
using SampleApplication.AppServices;
using System;
using System.Threading.Tasks;

namespace SampleApplication
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            var landingPageNavigationService = CC.IoC.Resolve<ILandingPageNavigationService>();

            if (CC.Device.Platform == Platforms.iOS || CC.Device.Platform == Platforms.Windows)
            {
                Func<Task> task = async () =>
                {
                    await landingPageNavigationService.NavigateAsync().ConfigureAwait(false);
                };
                task().Wait();
            }
            else
            {
                landingPageNavigationService.NavigateAsync().ConfigureAwait(true);
            }

            InitializeComponent();
        }

        private INavigationService Navigation
        {
            get { return CC.IoC.Resolve<INavigationService>(); }
        }

        protected override async void OnResume()
        {
            // Handle when your app resumes
            await Navigation.ResumeAsync();
        }

        protected override async void OnSleep()
        {
            // Handle when your app sleeps
            await Navigation.SuspendAsync();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }
    }
}