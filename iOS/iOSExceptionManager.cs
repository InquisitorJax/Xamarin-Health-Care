using Core;
using System.Threading.Tasks;

namespace SampleApplication.iOS
{
    public class iOSExceptionManager : IPlatformExceptionManager
    {
        public Task ReportApplicationCrash()
        {
            return Task.FromResult(default(int));
        }
    }
}