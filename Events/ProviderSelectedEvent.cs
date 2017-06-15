using Core;
using Core.AppServices;
using Prism.Events;
using SampleApplication.Models;

namespace SampleApplication.Events
{
    public class ProviderSelectionMessageEvent : PubSubEvent<ProviderSelectionMessageResult>
    {
        public static void Publish(TaskResult result, HealthCareProvider provider)
        {
            var providerResult = new ProviderSelectionMessageResult(result) { SelectedProvider = provider };
            CC.EventMessenger.GetEvent<ProviderSelectionMessageEvent>().Publish(providerResult);
        }
    }

    public class ProviderSelectionMessageResult : ActionMessageResult
    {
        public ProviderSelectionMessageResult(TaskResult result) : base(result)
        {
        }

        public HealthCareProvider SelectedProvider { get; set; }
    }
}