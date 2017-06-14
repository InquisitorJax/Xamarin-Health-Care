using Core;
using Core.AppServices;
using Core.Controls;
using Prism.Commands;
using SampleApplication.Events;
using SampleApplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApplication.ViewModels
{
    public class AppointmentScheduleViewModel : ViewModelBase
    {
        private ObservableCollection<Appointment> _appointments;

        private bool _selectionMade;

        public AppointmentScheduleViewModel()
        {
            Appointments = new ObservableCollection<Appointment>();

            AddNewAppointmentTimeCommand = new DelegateCommand<DateTimeResult>(AddNewAppointmentTime);
        }

        public ICommand AddNewAppointmentTimeCommand { get; private set; }

        public ObservableCollection<Appointment> Appointments
        {
            get { return _appointments; }
            set { SetProperty(ref _appointments, value); }
        }

        public override void Closing()
        {
            if (!_selectionMade)
            {
                //NOTE: Always fire location selection event, so subscribers can unsubscribe
                AppointmentDateSelectionMessageEvent.Publish(TaskResult.Canceled, null);
            }
        }

        public override async Task InitializeAsync(Dictionary<string, string> args)
        {
            await base.InitializeAsync(args);

            await FetchProviderAppointmentsAsync();
        }

        private async void AddNewAppointmentTime(DateTimeResult newTime)
        {
            _selectionMade = true;

            if (newTime.SelectedDate < DateTime.Now)
            {
                await CC.UserNotifier.ShowMessageAsync("You can't select an appointment in the past", "Invalid Appointment");
            }
            else
            {
                AppointmentDateSelectionMessageEvent.Publish(TaskResult.Success, newTime);
                await Close();
            }
        }

        private Task<Notification> FetchProviderAppointmentsAsync()
        {
            var retResult = Notification.Success();

            Appointment existingAppointment = new Appointment();
            DateTime currentDate = DateTime.Now;
            DateTime startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 10, 0, 0);
            DateTime endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 12, 0, 0);
            existingAppointment.StartDateTime = startTime;
            existingAppointment.EndDateTime = endTime;
            existingAppointment.Name = "blocked meeting";

            Appointments.Add(existingAppointment);

            return Task.FromResult(retResult);
        }
    }
}