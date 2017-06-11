using System;
using System.Threading.Tasks;

namespace Core
{
    public interface IUserNotifier
    {
        Task ShowMessageAsync(string message, string caption, string acceptButtonText = "Ok");

        Task ShowToastAsync(string message, string caption = "", int durationInSeconds = 2);
    }
}