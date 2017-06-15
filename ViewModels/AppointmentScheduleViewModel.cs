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
using Xamarin.Forms;

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
            DateTime startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 11, 0, 0);
            DateTime endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 12, 0, 0);
            existingAppointment.StartDateTime = startTime;
            existingAppointment.EndDateTime = endTime;
            existingAppointment.Color = Color.FromHex("#262626");
            existingAppointment.Name = "existing provider appointment";

            Appointments.Add(existingAppointment);

            Appointment otherProviderApt = new Appointment();
            currentDate = DateTime.Now;
            startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 9, 0, 0);
            endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 10, 0, 0);
            otherProviderApt.Color = Color.FromHex("#2286C4");
            otherProviderApt.StartDateTime = startTime;
            otherProviderApt.EndDateTime = endTime;
            otherProviderApt.Name = "Jessica's Dentist Appointment";

            Appointments.Add(otherProviderApt);

            Appointment customerCalendarApt = new Appointment();
            currentDate = DateTime.Now;
            startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 13, 0, 0);
            endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 14, 0, 0);
            customerCalendarApt.Color = Color.FromHex("#249116");
            customerCalendarApt.StartDateTime = startTime;
            customerCalendarApt.EndDateTime = endTime;
            customerCalendarApt.Name = "Lunch with David";

            Appointments.Add(customerCalendarApt);

            return Task.FromResult(retResult);
        }
    }
}