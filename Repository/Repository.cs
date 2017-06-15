using Autofac;
using Core;
using SampleApplication.Models;
using SQLite;
using System;
using System.Threading.Tasks;

namespace SampleApplication
{
    public interface IRepository
    {
        Task<FetchModelResult<Appointment>> FetchAppointmentAsync(string id);

        Task<FetchModelCollectionResult<Appointment>> FetchAppointmentsAsync(string userId, string providerId);

        Task<FetchModelCollectionResult<HealthCareProvider>> FetchProvidersAsync(string providerId = null);

        Task<FetchModelResult<HealthCareUser>> GetCurrentUserAsync();

        Task InitializeAsync();

        Task<Notification> SaveAppointmentAsync(Appointment item, ModelUpdateEvent updateEvent);

        Task<Notification> SaveCurrentUserAsync(HealthCareUser user, ModelUpdateEvent updateEvent);

        Task<Notification> SaveProviderAsync(HealthCareProvider item, ModelUpdateEvent updateEvent);
    }

    public class Repository : IRepository
    {
        #region IRepository implementation

        private readonly string _currentUserId = "e35fd1d9-6aaf-4c89-8d68-509505582f43";
        private SQLiteAsyncConnection _database;

        private bool _isInitialized = false;

        public async Task<FetchModelResult<Appointment>> FetchAppointmentAsync(string id)
        {
            FetchModelResult<Appointment> retResult = new FetchModelResult<Appointment>();

            var item = await _database.FindAsync<Appointment>(id);
            retResult.Model = item;

            return retResult;
        }

        public async Task<FetchModelCollectionResult<Appointment>> FetchAppointmentsAsync(string userId, string providerId)
        {
            FetchModelCollectionResult<Appointment> retResult = new FetchModelCollectionResult<Appointment>();

            var query = _database.Table<Appointment>();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (!string.IsNullOrWhiteSpace(providerId))
            {
                query = query.Where(x => x.ProviderId == providerId);
            }

            var items = await query.ToListAsync();

            retResult.ModelCollection = items;
            return retResult;
        }

        public async Task<FetchModelCollectionResult<HealthCareProvider>> FetchProvidersAsync(string providerId = null)
        {
            FetchModelCollectionResult<HealthCareProvider> retResult = new FetchModelCollectionResult<HealthCareProvider>();

            try
            {
                var query = _database.Table<HealthCareProvider>();
                if (!string.IsNullOrWhiteSpace(providerId))
                {
                    query = query.Where(x => x.Id == providerId);
                }

                var items = await query.ToListAsync();

                retResult.ModelCollection = items;
            }
            catch (Exception)
            {
                retResult.Notification.Add("Error fetching providers");
            }

            return retResult;
        }

        public async Task<FetchModelResult<HealthCareUser>> GetCurrentUserAsync()
        {
            var retResult = new FetchModelResult<HealthCareUser>();

            var item = await _database.FindAsync<HealthCareUser>(_currentUserId);
            retResult.Model = item;

            return retResult;
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;

            var connectionFactory = CC.IoC.Resolve<IDatabaseConnectionFactory>();
            var connectionResult = connectionFactory.Execute(null);
            if (connectionResult.IsValid())
            {
                _database = connectionResult.Connection;
                await _database.CreateTableAsync<Appointment>();
                await _database.CreateTableAsync<HealthCareUser>();
                await _database.CreateTableAsync<HealthCareProvider>();

                var user = await CheckCurrentUser();

                await SeedSampleDataAsync(user.Id);
            }
        }

        public async Task<Notification> SaveAppointmentAsync(Appointment item, ModelUpdateEvent updateEvent)
        {
            if (updateEvent == ModelUpdateEvent.Created)
            {
                item.UserId = _currentUserId;
            }
            return await SaveItem(item, updateEvent);
        }

        public async Task<Notification> SaveCurrentUserAsync(HealthCareUser item, ModelUpdateEvent updateEvent)
        {
            return await SaveItem(item, updateEvent);
        }

        public async Task<Notification> SaveProviderAsync(HealthCareProvider item, ModelUpdateEvent updateEvent)
        {
            return await SaveItem(item, updateEvent);
        }

        private async Task<HealthCareUser> CheckCurrentUser()
        {
            HealthCareUser retUser = null;
            var result = await GetCurrentUserAsync();
            retUser = result.Model;
            if (retUser == null)
            {
                retUser = new HealthCareUser { Id = _currentUserId, Name = "Jack Family", Description = "Dedicated to improving our health" };
                await _database.InsertAsync(retUser);
            }

            return retUser;
        }

        private async Task<Notification> SaveItem<T>(T item, ModelUpdateEvent updateEvent) where T : ModelBase
        {
            Notification retNotification = Notification.Success();
            try
            {
                if (updateEvent == ModelUpdateEvent.Created)
                {
                    if (string.IsNullOrWhiteSpace(item.Id))
                    {
                        item.Id = Guid.NewGuid().ToString();
                    }
                    await _database.InsertAsync(item);
                }
                else
                {
                    await _database.UpdateAsync(item);
                }
            }
            catch (SQLiteException)
            {
                //LOG:
                retNotification.Add(new NotificationItem("Save Failed"));
            }

            return retNotification;
        }

        private async Task SeedSampleDataAsync(string currentUserId)
        {
            //providers
            var fetchProvidersResult = await FetchProvidersAsync();

            if (fetchProvidersResult.IsValid() && fetchProvidersResult.ModelCollection.Count == 0)
            {
                const string facebook = "https://www.facebook.com/cliniko";
                var provider1 = new HealthCareProvider { Name = "Doctor Strange", Description = "For all your mystical needs", ImageName = "strange", PhoneNumber = "555 1234", FacebookUrl = facebook };
                var provider2 = new HealthCareProvider { Name = "Doctor Manhattan", Description = "For everything in the DC universe that needs fixing", ImageName = "manhattan", PhoneNumber = "555 1441", FacebookUrl = facebook };
                var provider3 = new HealthCareProvider { Name = "Doctor Fate", Description = "Don't pay attention to Strange - I am the true master!", ImageName = "fate", PhoneNumber = "555 7777", FacebookUrl = facebook };
                await SaveProviderAsync(provider1, ModelUpdateEvent.Created);
                await SaveProviderAsync(provider2, ModelUpdateEvent.Created);
                await SaveProviderAsync(provider3, ModelUpdateEvent.Created);

                //appointments
                var fetchAppointmentsResult = await FetchAppointmentsAsync(currentUserId, null);

                if (fetchAppointmentsResult.IsValid() && fetchAppointmentsResult.ModelCollection.Count == 0)
                {
                    var appointment1 = new Appointment { Name = "Doctor Strange apt", Description = "Need some help with dormamu infection", AppointmentDate = DateTime.Now.AddDays(2), ProviderId = provider1.Id, UserId = currentUserId, ProviderImageName = "strange.png" };
                    var appointment2 = new Appointment { Name = "Doctor Manhattan apt", Description = "Need diagnosis on infection effecting DC superheroes", AppointmentDate = DateTime.Now.AddDays(4), ProviderId = provider2.Id, UserId = currentUserId, ProviderImageName = "manhattan.png" };
                    await SaveAppointmentAsync(appointment1, ModelUpdateEvent.Created);
                    await SaveAppointmentAsync(appointment2, ModelUpdateEvent.Created);
                }
            }
        }

        #endregion IRepository implementation
    }
}