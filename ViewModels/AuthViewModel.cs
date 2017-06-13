using Core;
using Prism.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApplication.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        private IRepository _repo;

        public AuthViewModel(IRepository repo)
        {
            OpenTermsOfServiceCommand = new DelegateCommand(OpenTermsOfService);
            LoginWithFacebookCommand = new DelegateCommand(Login);
            LoginWithGoogleCommand = new DelegateCommand(Login);

            _repo = repo;
        }

        public ICommand LoginWithFacebookCommand { get; private set; }
        public ICommand LoginWithGoogleCommand { get; private set; }
        public ICommand OpenTermsOfServiceCommand { get; private set; }

        private async void Login()
        {
            await LoginAsync();
        }

        private async Task LoginAsync()
        {
            //TODO: Can use Xamarin.Auth to plugin actual oAuth social authentication
            //https://github.com/xamarin/Xamarin.Auth

            var user = await _repo.GetCurrentUserAsync();

            user.Model.IsLoggedIn = true;

            await _repo.SaveCurrentUserAsync(user.Model, ModelUpdateEvent.Updated);

            await CC.Navigation.NavigateAsync(Constants.Navigation.MainPage);
        }

        private void OpenTermsOfService()
        {
            const string url = "https://www.google.co.za/?gfe_rd=cr&ei=aQA_WZziOo6p8wfeu6y4CQ#q=terms+of+service";
            CC.Device.OpenUri(new Uri(url));
        }
    }
}