using Autofac;
using Core;
using Core.Controls;
using Prism.Commands;
using Prism.Events;
using SampleApplication.Events;
using SampleApplication.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApplication.ViewModels
{
    public class AppointmentViewModel : ViewModelBase
    {
        private readonly IRepository _repository;
        private readonly IModelValidator<Appointment> _validator;

        private bool _isNewModel;
        private GeoLocation _location;
        private Appointment _model;
        private HealthCareProvider _provider;
        private SubscriptionToken _providerSelectionToken;
        private SubscriptionToken _selectionToken;

        public AppointmentViewModel(IRepository repository, IModelValidator<Appointment> validator)
        {
            _repository = repository;
            _validator = validator;

            Locations = new ObservableCollection<GeoLocation>();
            PhoneProviderCommand = new DelegateCommand(PhoneProvider);
            SaveItemCommand = new DelegateCommand(SaveItem);
            SelectAppointmentCommand = new DelegateCommand(SelectAppointment);
            SelectProviderCommand = new DelegateCommand(SelectProvider);
            SelectPatientCommand = new DelegateCommand(SelectPatient);
            ShareCommand = new DelegateCommand(Share);

            _selectionToken = CC.EventMessenger.GetEvent<AppointmentDateSelectionMessageEvent>().Subscribe(OnAppointmentDateSelected);
            _providerSelectionToken = CC.EventMessenger.GetEvent<ProviderSelectionMessageEvent>().Subscribe(OnProviderSelected);
        }

        public GeoLocation Location
        {
            get { return _location; }
            set
            {
                SetProperty(ref _location, value);
                Locations.Clear();
                if (value != null)
                {
                    Locations.Add(value);
                }
            }
        }

        public ObservableCollection<GeoLocation> Locations { get; set; }

        public Appointment Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        public ICommand PhoneProviderCommand { get; private set; }

        public HealthCareProvider Provider
        {
            get { return _provider; }
            set
            {
                SetProperty(ref _provider, value);
                if (_provider != null)
                {
                    Location = GeoLocation.FromWellKnownText(_provider.Location);
                }
            }
        }

        public ICommand SaveItemCommand { get; private set; }

        public ICommand SelectAppointmentCommand { get; private set; }

        public ICommand SelectPatientCommand { get; private set; }

        public ICommand SelectProviderCommand { get; private set; }

        public ICommand ShareCommand { get; private set; }

        public override void Closing()
        {
            CC.EventMessenger.GetEvent<AppointmentDateSelectionMessageEvent>().Unsubscribe(_selectionToken);
            CC.EventMessenger.GetEvent<ProviderSelectionMessageEvent>().Unsubscribe(_providerSelectionToken);

            base.Closing();
        }

        public override async Task InitializeAsync(System.Collections.Generic.Dictionary<string, string> args)
        {
            if (args != null && args.ContainsKey(Constants.Parameters.Id))
            {
                string id = args[Constants.Parameters.Id];
                var fetchResult = await _repository.FetchAppointmentAsync(id);
                if (fetchResult.IsValid())
                {
                    Model = fetchResult.Model;

                    if (Model != null)
                    {
                        var providerResult = await _repository.FetchProvidersAsync(Model.ProviderId);
                        if (providerResult.IsValid() && providerResult.ModelCollection.Count > 0)
                        {
                            Provider = providerResult.ModelCollection.First();
                        }
                    }
                }
                else
                {
                    await UserNotifier.ShowMessageAsync(fetchResult.Notification.ToString(), "Fetch Error");
                }
            }
            else
            { //assume new model required
                Model = new Appointment()
                {
                    Id = Guid.NewGuid().ToString()
                };
                _isNewModel = true;
            }
        }

        private void OnAppointmentDateSelected(AppointmentDateMessageResult result)
        {
            if (result.Result == Core.AppServices.TaskResult.Success)
            {
                Model.AppointmentDate = result.DateResult.SelectedDate;
            }
        }

        private void OnProviderSelected(ProviderSelectionMessageResult result)
        {
            if (result.Result == Core.AppServices.TaskResult.Success)
            {
                Provider = result.SelectedProvider;
                Model.ProviderImageName = Provider.ImageName;
            }
        }

        private void PhoneProvider()
        {
            CC.Device.OpenUri(new Uri(String.Format("tel:{0}", Provider.PhoneNumber)));
        }

        private async void SaveItem()
        {
            await SaveItemAsync();
        }

        private async Task SaveItemAsync()
        {
            Notification result = Notification.Success();
            ModelUpdateEvent updateEvent = _isNewModel ? ModelUpdateEvent.Created : ModelUpdateEvent.Updated;

            result = _validator.ValidateModel(Model);

            if (result.IsValid())
            {
                var saveResult = await _repository.SaveAppointmentAsync(Model, updateEvent);
                result.AddRange(saveResult);
            }

            if (result.IsValid())
            {
                var eventMessenger = CC.IoC.Resolve<IEventAggregator>();
                ModelUpdatedMessageResult<Appointment> eventResult = new ModelUpdatedMessageResult<Appointment>() { UpdatedModel = Model, UpdateEvent = updateEvent };
                eventMessenger.GetEvent<ModelUpdatedMessageEvent<Appointment>>().Publish(eventResult);
                await Close();
            }
            else
            {
                await UserNotifier.ShowMessageAsync(result.ToString(), "Save Failed");
            }
        }

        private async void SelectAppointment()
        {
            await CC.Navigation.NavigateAsync(Constants.Navigation.AppointmentSchedulePage);
        }

        private async void SelectPatient()
        {
            await CC.UserNotifier.ShowMessageAsync("TODO: select a family member for the appointment", "under construction");
        }

        private async void SelectProvider()
        {
            await CC.Navigation.NavigateAsync(Constants.Navigation.ProviderListPage);
        }

        private void Share()
        {
            string message = "@cliniko care is awesome! service from " + Provider.Name + " was fantastic!";
            string title = "cliniko care";

            CC.Device.Share(message, title, Provider.FacebookUrl);
        }
    }
}