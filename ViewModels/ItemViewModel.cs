using Autofac;
using Core;
using Prism.Commands;
using Prism.Events;
using SampleApplication.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApplication.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        private readonly IRepository _repository;
        private readonly IModelValidator<Appointment> _validator;

        private bool _isNewModel;

        private Appointment _model;

        public ItemViewModel(IRepository repository, IModelValidator<Appointment> validator)
        {
            _repository = repository;
            _validator = validator;
            SaveItemCommand = new DelegateCommand(SaveItem);
        }

        public Appointment Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        public ICommand SaveItemCommand { get; private set; }

        public override async Task InitializeAsync(System.Collections.Generic.Dictionary<string, string> args)
        {
            if (args != null && args.ContainsKey(Constants.Parameters.Id))
            {
                string id = args[Constants.Parameters.Id];
                var fetchResult = await _repository.FetchAppointmentAsync(id);
                if (fetchResult.IsValid())
                {
                    Model = fetchResult.Model;
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
    }
}