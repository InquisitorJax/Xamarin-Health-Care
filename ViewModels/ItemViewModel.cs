using Autofac;
using Core;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApplication
{
    public class ItemViewModel : ViewModelBase
    {
        private readonly IRepository _repository;
        private readonly IModelValidator<SampleItem> _validator;

        private bool _isNewModel;

        private SampleItem _model;

        public ItemViewModel(IRepository repository, IModelValidator<SampleItem> validator)
        {
            _repository = repository;
            _validator = validator;
            SaveItemCommand = new DelegateCommand(SaveItem);
        }

        public SampleItem Model
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
                var fetchResult = await _repository.FetchSampleItemAsync(id);
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
                Model = new SampleItem()
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
                var saveResult = await _repository.SaveSampleItemAsync(Model, updateEvent);
                result.AddRange(saveResult);
            }

            if (result.IsValid())
            {
                var eventMessenger = CC.IoC.Resolve<IEventAggregator>();
                ModelUpdatedMessageResult<SampleItem> eventResult = new ModelUpdatedMessageResult<SampleItem>() { UpdatedModel = Model, UpdateEvent = updateEvent };
                eventMessenger.GetEvent<ModelUpdatedMessageEvent<SampleItem>>().Publish(eventResult);
                await Close();
            }
            else
            {
                await UserNotifier.ShowMessageAsync(result.ToString(), "Save Failed");
            }
        }
    }
}