
using Azure.Messaging.ServiceBus;

namespace Core.CoreLib.Services.Azure.ServiceBus.Processors
{
    public interface IServiceBusMessageProcessor
    {
        Task ProcessMessageAsync(ProcessMessageEventArgs args);
        string Subject { get; }
        string ConnectionString { get; }
        string Topic { get; }
        string Subscription { get; }
    }
}