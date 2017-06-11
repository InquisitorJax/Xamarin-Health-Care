using Autofac;
using Core;

namespace SampleApplication.Windows
{
    public class IocWindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<WindowsDatabaseConnectionFactory>().As<IDatabaseConnectionFactory>().AsSelf();
            builder.RegisterType<WindowsExceptionManager>().As<IPlatformExceptionManager>().AsSelf().SingleInstance();
        }
    }
}