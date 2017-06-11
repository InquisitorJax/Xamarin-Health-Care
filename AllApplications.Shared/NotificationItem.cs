using System;

namespace Core
{
    public enum NotificationSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }

    public class NotificationItem
    {
        public NotificationItem(string message, NotificationSeverity severity = NotificationSeverity.Error)
        {
            Severity = severity;
            Message = message;
        }

        public string Message { get; set; }
        public string Reference { get; set; }
        public NotificationSeverity Severity { get; set; }

        public Notification AsNotification()
        {
            var notification = new Notification();
            notification.Add(Message, Severity);
            return notification;
        }
    }
}