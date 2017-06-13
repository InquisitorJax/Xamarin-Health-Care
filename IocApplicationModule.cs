using Autofac;
using Core;
using SampleApplication.AppServices;
using SampleApplication.Models;
using SampleApplication.ViewModels;
using SampleApplication.Views;

namespace SampleApplication
{
    public class IocApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Repository>().As<IRepository>().AsSelf().SingleInstance();

            builder.RegisterType<LandingPageNavigationService>().As<ILandingPageNavigationService>().AsSelf().SingleInstance();

            builder.RegisterType<AppointmentValidator>().As<IModelValidator<Appointment>>().AsSelf();

            builder.RegisterType<MainViewModel>().Named<IViewModel>(Constants.Navigation.MainPage);
            builder.RegisterType<AppointmentViewModel>().Named<IViewModel>(Constants.Navigation.AppointmentPage);
            builder.RegisterType<AuthViewModel>().Named<IViewModel>(Constants.Navigation.AuthPage);

            builder.RegisterType<MainPage>().Named<IView>(Constants.Navigation.MainPage);
            builder.RegisterType<AuthPage>().Named<IView>(Constants.Navigation.AuthPage);
            builder.RegisterType<AppointmentPage>().Named<IView>(Constants.Navigation.AppointmentPage);
        }
    }
}