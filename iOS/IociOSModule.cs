using Autofac;
using Core;

namespace SampleApplication.iOS
{
    public class IociOSModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<iOSDatabaseConnectionFactory>().As<IDatabaseConnectionFactory>().AsSelf();
            builder.RegisterType<iOSExceptionManager>().As<IPlatformExceptionManager>().AsSelf().SingleInstance();
        }
    }
}