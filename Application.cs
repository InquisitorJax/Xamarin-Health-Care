using Autofac;
using Core;
using System.Collections.Generic;

namespace SampleApplication
{
    public class App : Xamarin.Forms.Application
    {
        public App(Module platformModule)
        {
            //TODO: Incorporate Splash screen to await initialization, and then navigation to main page
            Initialize(platformModule);
            // The root page of your application
            Navigation.NavigateAsync(Constants.Navigation.MainPage);
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

        private void Initialize(Module platformModule)
        {
            //IOC
            List<Module> modules = new List<Module>
            {
                new IocSharedModule(),
                new IocApplicationModule(),
                platformModule
            };

            CC.InitializeIoc(modules.ToArray());

            //REPO
            var repository = CC.IoC.Resolve<IRepository>();
            repository.Initialize();

            //Exception Manager
            var exceptionManager = CC.IoC.Resolve<IPlatformExceptionManager>();
            exceptionManager.ReportApplicationCrash();
        }
    }
}