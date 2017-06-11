using Plugin.Toasts;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Core
{
    public class XFormsUserNotifier : IUserNotifier
    {
        #region IUserNotifier implementation

        public async Task ShowMessageAsync(string message, string caption, string acceptButtonText = "Ok")
        {
            Page currentPage = (Page)CC.Navigation.Current;
            await currentPage.DisplayAlert(caption, message, acceptButtonText);
        }

        public async Task ShowToastAsync(string message, string caption = "", int durationInSeconds = 2)
        {
            TimeSpan duration = TimeSpan.FromSeconds(durationInSeconds);
            var notificator = DependencyService.Get<IToastNotificator>();
            var options = new NotificationOptions() { Description = message, Title = caption };
            await notificator.Notify(options);
        }

        #endregion IUserNotifier implementation
    }
}