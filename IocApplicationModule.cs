using Autofac;
using Core;
using SampleApplication.Views;

namespace SampleApplication
{
    public class IocApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Repository>().As<IRepository>().AsSelf().SingleInstance();

            builder.RegisterType<SampleItemValidator>().As<IModelValidator<SampleItem>>().AsSelf();

            builder.RegisterType<MainViewModel>().Keyed<IViewModel>(Constants.Navigation.MainPage);
            builder.RegisterType<ItemViewModel>().Keyed<IViewModel>(Constants.Navigation.ItemPage);

            builder.RegisterType<MainPage>().Keyed<IView>(Constants.Navigation.MainPage);
            builder.RegisterType(typeof(ItemPage)).Keyed(Constants.Navigation.ItemPage, typeof(IView));
        }
    }
}