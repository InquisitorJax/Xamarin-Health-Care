using Core;
using Core.Controls;
using Prism.Commands;
using SampleApplication.Events;
using SampleApplication.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApplication.ViewModels
{
    public class ProviderListViewModel : ViewModelBase
    {
        private readonly IAppCache _appCache;
        private readonly IRepository _repo;

        private ObservableCollection<HealthCareProvider> _providers;

        private HealthCareProvider _selectedProvider;

        public ProviderListViewModel(IRepository repo, IAppCache appCache)
        {
            _repo = repo;
            _appCache = appCache;

            SelectProviderCommand = new DelegateCommand<HealthCareProvider>(SelectProvider);
        }

        public ObservableCollection<HealthCareProvider> Providers
        {
            get { return _providers; }
            set { SetProperty(ref _providers, value); }
        }

        public HealthCareProvider SelectedProvider
        {
            get { return _selectedProvider; }
            set { SetProperty(ref _selectedProvider, value); }
        }

        public ICommand SelectProviderCommand { get; private set; }

        public override async Task InitializeAsync(Dictionary<string, string> args)
        {
            await base.InitializeAsync(args);

            await FetchProvidersAsync();
        }

        private async Task<Notification> FetchProvidersAsync()
        {
            var retResult = Notification.Success();
            ShowBusy("loading providers...");

            try
            {
                var providersResult = await _repo.FetchProvidersAsync();

                if (providersResult.IsValid())
                {
                    var providers = providersResult.ModelCollection;
                    if (_appCache.CurrentLocation != null)
                    {
                        //calculate distances from current location
                        var currentLocation = _appCache.CurrentLocation;

                        //sort by said distance
                        foreach (var provider in providers)
                        {
                            if (!string.IsNullOrEmpty(provider.Location))
                            {
                                var providerLocation = GeoLocation.FromWellKnownText(provider.Location);
                                provider.DistanceFromCurrentLocation = providerLocation.DistanceFrom(currentLocation);
                            }
                        }

                        providers = providers.OrderBy(x => x.DistanceFromCurrentLocation).ToList();
                    }

                    Providers = providers.AsObservableCollection();
                }
            }
            finally
            {
                NotBusy();
            }

            return retResult;
        }

        private async void SelectProvider(HealthCareProvider provider)
        {
            ProviderSelectionMessageEvent.Publish(Core.AppServices.TaskResult.Success, provider);

            await Close();
        }
    }
}