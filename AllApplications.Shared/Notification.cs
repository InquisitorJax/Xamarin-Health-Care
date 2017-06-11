using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Notification
    {
        public Notification()
        {
            Items = new List<NotificationItem>();
        }

        public Notification(string description) : base()
        {
        }

        public string Description { get; set; }

        public bool HasError
        {
            get { return Items.Any(x => x.Severity == NotificationSeverity.Error); }
        }

        public IList<NotificationItem> Items { get; private set; }

        public NotificationSeverity Severity { get; private set; }

        public static Notification Error(string errorMessage)
        {
            var retNotification = new Notification();
            retNotification.Add(errorMessage);
            return retNotification;
        }

        public static Notification Success()
        {
            return new Notification();
        }

        public void Add(string message, NotificationSeverity severity = NotificationSeverity.Error)
        {
            var item = new NotificationItem(message, severity);
            Items.Add(item);
        }

        public void Add(NotificationItem notificationItem)
        {
            Items.Add(notificationItem);
        }

        public void AddRange(Notification notification)
        {
            foreach (var item in notification.Items)
            {
                Items.Add(item);
            }
        }

        public bool IsValid(NotificationSeverity severity = NotificationSeverity.Error)
        {
            //return Severity < severity;
            return Items.All(x => x.Severity < severity);
        }

        public override string ToString()
        {
            string retNotification = Description;

            foreach (var item in Items)
            {
                retNotification += item.Message + Environment.NewLine;
            }

            return retNotification;
        }
    }
}