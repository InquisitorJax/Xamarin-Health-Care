using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core
{
    public interface IViewModel
    {
        void Closing();

        Task InitializeAsync(Dictionary<string, string> args);

        Task LoadStateAsync();

        Task SaveStateAsync();
    }
}