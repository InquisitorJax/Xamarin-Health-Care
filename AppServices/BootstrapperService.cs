using Autofac;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApplication.AppServices
{
    public class BootstrapperService
    {
        public static async Task Initialize(Module platformModule)
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
            await repository.InitializeAsync();
        }
    }
}