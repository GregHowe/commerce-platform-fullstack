
using System.Text.Json;

namespace Core.CoreLib.Services.Azure.ServiceBus
{
    public interface IMessageSender
    {
        Task SendMessageAsync<T>(
            T messages,
            string serviceBusConnectionString,
            string topic,
            JsonNamingPolicy? jsonNamingPolicy = null);
    }
}