using Core;
using System.Threading.Tasks;

namespace SampleApplication.Windows
{
    public class WindowsExceptionManager : IPlatformExceptionManager
    {
        public Task ReportApplicationCrash()
        {
            return Task.FromResult(default(int));
        }
    }
}