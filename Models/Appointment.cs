using Core;
using System;

namespace SampleApplication.Models
{
    public class Appointment : ModelBase
    {
        private DateTime? _appointmentDate;
        private string _description;
        private string _name;

        private string _providerId;

        private string _userId;

        public DateTime? AppointmentDate
        {
            get { return _appointmentDate; }
            set { SetProperty(ref _appointmentDate, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
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