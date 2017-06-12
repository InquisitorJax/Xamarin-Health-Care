using Core;
using System.Threading.Tasks;

namespace SampleApplication.AppServices
{
    public interface ILandingPageNavigationService
    {
        Task NavigateAsync();
    }

    public class LandingPageNavigationService : ILandingPageNavigationService
    {
        private readonly INavigationService _navService;
        private readonly IRepository _repo;

        public LandingPageNavigationService(IRepository repo, INavigationService navService)
        {
            _repo = repo;
            _navService = navService;
        }

        public async Task NavigateAsync()
        {
            var currentUserResult = await _repo.GetCurrentUserAsync();

            if (currentUserResult.Model != null && currentUserResult.Model.IsLoggedIn)
            {
                await _navService.NavigateAsync(Constants.Navigation.MainPage);
            }
            else
            {
                await _navService.NavigateAsync(Constants.Navigation.AuthPage);
            }
        }
    }
}