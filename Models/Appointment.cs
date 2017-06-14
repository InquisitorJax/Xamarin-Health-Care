using Core;
using Core.Controls;
using SQLite;
using System;
using Xamarin.Forms;

namespace SampleApplication.Models
{
    public class Appointment : ModelBase, IScheduleAppointment
    {
        private DateTime? _appointmentDate;
        private Color _color;
        private string _description;
        private DateTime? _from;
        private string _name;

        private string _providerId;

        private string _providerImageName;
        private DateTime? _to;
        private string _userId;

        public DateTime? AppointmentDate
        {
            get { return _appointmentDate; }
            set
            {
                SetProperty(ref _appointmentDate, value);
                StartDateTime = _appointmentDate;
                if (StartDateTime.HasValue)
                {
                    EndDateTime = StartDateTime.Value.AddHours(1);
                }
            }
        }

        [Ignore]
        public Color Color
        {
            get { return Color.FromHex("#2C92D1"); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public DateTime? EndDateTime
        {
            get { return _to; }
            set { SetProperty(ref _to, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string ProviderId
        {
            get { return _providerId; }
            set { SetProperty(ref _providerId, value); }
        }

        public string ProviderImageName
        {
            get { return _providerImageName; }
            set { SetProperty(ref _providerImageName, value); }
        }

        public DateTime? StartDateTime
        {
            get { return _from; }
            set { SetProperty(ref _from, value); }
        }

        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        public static Appointment Create(string userId, string providerId)
        {
            return new Appointment { UserId = userId, ProviderId = providerId };
        }
    }
}