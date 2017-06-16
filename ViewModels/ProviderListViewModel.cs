using Core;
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

        private bool _isForSelection;
        private ObservableCollection<HealthCareProvider> _providers;

        private HealthCareProvider _selectedProvider;

        public ProviderListViewModel(IRepository repo, IAppCache appCache)
        {
            _repo = repo;
            _appCache = appCache;

            SelectProviderCommand = new DelegateCommand<HealthCareProvider>(SelectProvider);
            PinProviderCommand = new DelegateCommand<HealthCareProvider>(PinProvider);
            NewAppointmentCommand = new DelegateCommand(CreateNewAppointment);
        }

        public bool IsForSelection
        {
            get { return _isForSelection; }
            set { SetProperty(ref _isForSelection, value); }
        }

        public ICommand NewAppointmentCommand { get; private set; }

        public ICommand PinProviderCommand { get; private set; }

        public ObservableCollection<HealthCareProvider> Providers
        {
            get { return _providers; }
            set { SetProperty(ref _providers, value); }
        }

        public HealthCareProvider SelectedProvider
        {
            get { return _selectedProvider; }
            set
            {
                SetProperty(ref _selectedProvider, value);
                UpdateSelection();
            }
        }

        public ICommand SelectProviderCommand { get; private set; }

        public override async Task InitializeAsync(Dictionary<string, string> args)
        {
            await base.InitializeAsync(args);

            _isForSelection = args != null && args.ContainsKey(Constants.Parameters.ForSelection);

            await FetchProvidersAsync();
        }

        private async void CreateNewAppointment()
        {
            var args = new Dictionary<string, string>();
            if (SelectedProvider != null)
            {
                args.Add(Constants.Parameters.ProviderId, SelectedProvider.Id);
            }
            await CC.Navigation.NavigateAsync(Constants.Navigation.AppointmentPage, args, false, true);
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
                    providers = providers.OrderByDescending(x => x.IsPinned).ThenBy(y => y.DistanceFromCurrentLocation).ToList();

                    Providers = providers.AsObservableCollection();
                }
            }
            finally
            {
                NotBusy();
            }

            return retResult;
        }

        private void PinProvider(HealthCareProvider provider)
        {
            if (provider != null)
            {
                provider.IsPinned = !provider.IsPinned;
                Providers = Providers.OrderByDescending(x => x.IsPinned).ThenBy(y => y.DistanceFromCurrentLocation).ToList().AsObservableCollection();
                _repo.SaveProviderAsync(provider, ModelUpdateEvent.Updated);
            }
        }

        private async void SelectProvider(HealthCareProvider provider)
        {
            if (_isForSelection)
            {
                ProviderSelectionMessageEvent.Publish(Core.AppServices.TaskResult.Success, provider);

                await Close();
            }
        }

        private void UpdateSelection()
        {
            if (Providers != null)
            {
                foreach (var provider in Providers)
                {
                    provider.IsSelected = provider == SelectedProvider;
                }
            }
        }
    }
}