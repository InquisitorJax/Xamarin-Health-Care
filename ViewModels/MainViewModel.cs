using Core;
using Prism.Commands;
using Prism.Events;
using SampleApplication.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApplication.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRepository _repository;

        private HealthCareUser _currentUser;
        private bool _listRefreshing;

        private bool _mainMenuOpen;
        private SubscriptionToken _modelUpdatedEventToken;

        private ObservableCollection<SampleItem> _sampleItems;

        private SampleItem _selectedSampleItem;

        public MainViewModel(IRepository repository)
        {
            _repository = repository;
            FetchSampleItemsCommand = new DelegateCommand(FetchSampleItems);
            OpenSelectedSampleItemCommand = new DelegateCommand(OpenSelectedSampleItem);
            CreateSampleItemNavigationCommand = new DelegateCommand(CreateSampleItemNavigate);
            MainMenuItemClickCommand = new DelegateCommand<MainMenuItem>(MainMenuItemClick);
            Title = "Cliniko Care";

            MainMenuItems = new List<MainMenuItem>();
            MainMenuItems.Add(new MainMenuItem
            {
                Title = "Cliniko Health Providers",
                IconSource = "circle_logo.png",
                ActionId = Constants.Navigation.ProviderSearchPage
            });
            MainMenuItems.Add(new MainMenuItem
            {
                Title = "Log out",
                IconSource = "logout_dark.png",
                ActionId = Constants.Navigation.Logout
            });

            //MainMenuItems.Add(new MainMenuItem
            //{
            //    Title = "About",
            //    IconSource = "greenlantern.png",
            //    ActionId = Constants.Navigation.AboutPage;
            //});
        }

        public ICommand CreateSampleItemNavigationCommand { get; private set; }

        public HealthCareUser CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        public ICommand FetchSampleItemsCommand { get; private set; }

        public bool ListRefreshing
        {
            get { return _listRefreshing; }
            set { SetProperty(ref _listRefreshing, value); }
        }

        public ICommand MainMenuItemClickCommand { get; private set; }

        public IList<MainMenuItem> MainMenuItems { get; private set; }

        public bool MainMenuOpen
        {
            get { return _mainMenuOpen; }
            set { SetProperty(ref _mainMenuOpen, value); }
        }

        public ICommand OpenSelectedSampleItemCommand { get; private set; }

        public ObservableCollection<SampleItem> SampleItems
        {
            get { return _sampleItems; }
            set { SetProperty(ref _sampleItems, value); }
        }

        public SampleItem SelectedSampleItem
        {
            get { return _selectedSampleItem; }
            set { SetProperty(ref _selectedSampleItem, value); }
        }

        public string Title { get; set; }

        public override void Closing()
        {
            CC.EventMessenger.GetEvent<ModelUpdatedMessageEvent<SampleItem>>().Unsubscribe(_modelUpdatedEventToken);
        }

        public override async Task InitializeAsync(System.Collections.Generic.Dictionary<string, string> args)
        {
            _modelUpdatedEventToken = CC.EventMessenger.GetEvent<ModelUpdatedMessageEvent<SampleItem>>().Subscribe(OnSampleItemUpdated);
            await FetchCurrentUserAsync();
            await FetchSampleItemsAsync();
        }

        private async void CreateSampleItemNavigate()
        {
            await Navigation.NavigateAsync(Constants.Navigation.HealthCareProviderPage);
        }

        private async Task FetchCurrentUserAsync()
        {
            var fetchResult = await _repository.GetCurrentUserAsync();

            CurrentUser = fetchResult.Model;
        }

        private async void FetchSampleItems()
        {
            await FetchSampleItemsAsync();
        }

        private async Task FetchSampleItemsAsync()
        {
            ListRefreshing = true;

            try
            {
                FetchModelCollectionResult<SampleItem> fetchResult = await _repository.FetchSampleItemsAsync();

                if (fetchResult.IsValid())
                {
                    SampleItems = fetchResult.ModelCollection.AsObservableCollection();

                    ListRefreshing = false;
                }
                else
                {
                    ListRefreshing = false;
                    await CC.UserNotifier.ShowMessageAsync(fetchResult.Notification.ToString(), "Fetch Sample Items Failed");
                }
            }
            finally
            {
                ListRefreshing = false;
            }
        }

        private async Task LogoutAsync()
        {
            if (CurrentUser != null)
            {
                CurrentUser.IsLoggedIn = false;
                await _repository.SaveCurrentUserAsync(CurrentUser);
            }
            await CC.Navigation.NavigateAsync(Constants.Navigation.AuthPage, null, false, false, true);
        }

        private async void MainMenuItemClick(MainMenuItem menuItem)
        {
            MainMenuOpen = false;

            if (menuItem != null)
            {
                switch (menuItem.ActionId)
                {
                    case Constants.Navigation.Logout:
                        await LogoutAsync();
                        break;
                }
            }
        }

        private void OnSampleItemUpdated(ModelUpdatedMessageResult<SampleItem> updateResult)
        {
            SampleItems.UpdateCollection(updateResult.UpdatedModel, updateResult.UpdateEvent);
        }

        private async void OpenSelectedSampleItem()
        {
            await OpenSelectedSampleItemAsync();
        }

        private async Task OpenSelectedSampleItemAsync()
        {
            if (SelectedSampleItem != null)
            {
                Dictionary<string, string> args = new Dictionary<string, string>
                {
                    {Constants.Parameters.Id, SelectedSampleItem.Id}
                };

                await Navigation.NavigateAsync(Constants.Navigation.HealthCareProviderPage, args);
            }
        }
    }
}