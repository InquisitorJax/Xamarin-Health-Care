using Autofac;
using Core.AppServices;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Prism.Events;

namespace Core
{
    public class IocSharedModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().AsSelf().SingleInstance();

            builder.RegisterType(typeof(XFormsNavigationService)).As(typeof(INavigationService)).AsSelf().SingleInstance();

            builder.RegisterType(typeof(XFormsUserNotifier)).As(typeof(IUserNotifier)).AsSelf();
            builder.RegisterType(typeof(Device)).As(typeof(IDevice)).AsSelf();
            builder.RegisterType(typeof(LocationService)).As(typeof(ILocationService)).AsSelf();

            //plugins
            builder.RegisterInstance(CrossGeolocator.Current).As<IGeolocator>();
        }
    }
}