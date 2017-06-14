using Core;
using Core.AppServices;
using Core.Controls;
using Prism.Events;

namespace SampleApplication.Events
{
    public class AppointmentDateMessageResult : ActionMessageResult
    {
        public AppointmentDateMessageResult(TaskResult result) : base(result)
        {
        }

        public DateTimeResult DateResult { get; set; }
    }

    public class AppointmentDateSelectionMessageEvent : PubSubEvent<AppointmentDateMessageResult>
    {
        public static void Publish(TaskResult result, DateTimeResult dateResult)
        {
            var messenger = CC.EventMessenger;
            var appointmentDateResult = new AppointmentDateMessageResult(result) { DateResult = dateResult };
            messenger.GetEvent<AppointmentDateSelectionMessageEvent>().Publish(appointmentDateResult);
        }
    }
}