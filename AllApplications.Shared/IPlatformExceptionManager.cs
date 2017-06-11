using System.Threading.Tasks;

namespace Core
{
    public interface IPlatformExceptionManager
    {
        Task ReportApplicationCrash();
    }
}