using Autofac;
using Core;
using SampleApplication.AppServices;
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

            builder.RegisterType<SampleItemValidator>().As<IModelValidator<SampleItem>>().AsSelf();

            builder.RegisterType<MainViewModel>().Named<IViewModel>(Constants.Navigation.MainPage);
            builder.RegisterType<ItemViewModel>().Named<IViewModel>(Constants.Navigation.HealthCareProviderPage);
            builder.RegisterType<AuthViewModel>().Named<IViewModel>(Constants.Navigation.AuthPage);

            builder.RegisterType<MainPage>().Named<IView>(Constants.Navigation.MainPage);
            builder.RegisterType<AuthPage>().Named<IView>(Constants.Navigation.AuthPage);
            builder.RegisterType(typeof(ItemPage)).Named(Constants.Navigation.HealthCareProviderPage, typeof(IView));
        }
    }
}