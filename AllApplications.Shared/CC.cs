using Autofac;
using Prism.Events;
using System;

namespace Core
{
    /// <summary>
    /// Central Command
    /// </summary>
    public static class CC
    {
        private static IComponentContext _container;

        static CC()
        {
        }

        public static IEventAggregator EventMessenger
        {
            get { return _container.Resolve<IEventAggregator>(); }
        }

        public static IComponentContext IoC
        {
            get { return _container; }
        }

        public static INavigationService Navigation
        {
            get { return _container.Resolve<INavigationService>(); }
        }

        public static IUserNotifier UserNotifier
        {
            get { return _container.Resolve<IUserNotifier>(); }
        }

        public static void InitializeIoc(Module[] modules)
        {
            var builder = new ContainerBuilder();
            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }
            _container = builder.Build();
        }
    }
}