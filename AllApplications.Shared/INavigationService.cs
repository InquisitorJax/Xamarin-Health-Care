using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core
{
    public interface INavigationService
    {
        object Current { get; }

        Task GoBack();

        Task NavigateAsync(string destination, Dictionary<string, string> args = null, bool modal = false, bool forgetCurrentPage = false);

        Task ResumeAsync();

        Task SuspendAsync();
    }
}