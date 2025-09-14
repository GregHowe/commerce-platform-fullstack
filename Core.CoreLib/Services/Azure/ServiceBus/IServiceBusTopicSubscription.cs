using Core.CoreLib.Models.Azure.ServiceBus;

namespace Core.CoreLib.Services.Azure.ServiceBus
{
    public interface IServiceBusTopicSubscription
    {
        Task HandleMessagesAsync();
        Task CloseSubscriptionAsync();
        ValueTask DisposeAsync();
        Task<Dictionary<string, int>> GetTopicDeadLetterCount();
    }
}
