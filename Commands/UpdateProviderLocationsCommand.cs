using Core;
using Core.AppServices;
using System.Threading.Tasks;

namespace SampleApplication.Commands
{
    public interface IUpdateProviderLocationsCommand : IAsyncLogicCommand<object, CommandResult>
    {
    }

    public class UpdateProviderLocationsCommand : AsyncLogicCommand<object, CommandResult>, IUpdateProviderLocationsCommand
    {
        private readonly ILocationService _locationService;
        private readonly IRepository _repo;

        public UpdateProviderLocationsCommand(IRepository repo, ILocationService locationService)
        {
            _repo = repo;
            _locationService = locationService;
        }

        public override async Task<CommandResult> ExecuteAsync(object request)
        {
            var retResult = new CommandResult();

            var providersResult = await _repo.FetchProvidersAsync();
            retResult.Notification.AddRange(providersResult.Notification);

            if (providersResult.IsValid())
            {
                var locationResult = await _locationService.FetchCurrentLocationAsync();
                retResult.Notification.AddRange(locationResult.Notification);

                if (locationResult.IsValid())
                {
                    var randomPoints = _locationService.GenerateRandomLocations(locationResult.CurrentLocation, 500, providersResult.ModelCollection.Count);
                    int index = 0;
                    foreach (var provider in providersResult.ModelCollection)
                    {
                        var location = randomPoints[index];

                        provider.Location = location.ToWellKnownText();
                        await _repo.SaveProviderAsync(provider, ModelUpdateEvent.Updated);

                        index++;
                    }
                }
            }

            return retResult;
        }
    }
}