using Autofac;
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
        }
    }
}