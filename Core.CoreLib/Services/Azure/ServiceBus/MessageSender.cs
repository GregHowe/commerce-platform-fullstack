using Azure.Messaging.ServiceBus;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.CoreLib.Services.Azure.ServiceBus
{
    public class MessageSender : IMessageSender
    {
        public async Task SendMessageAsync<T>(
            T message,
            string serviceBusConnectionString,
            string topic,
            JsonNamingPolicy? jsonNamingPolicy = null)
        {
            var client = new ServiceBusClient(serviceBusConnectionString);
            var clientSender = client.CreateSender(topic);

            try
            {
                using ServiceBusMessageBatch messageBatch = await clientSender.CreateMessageBatchAsync();

                var payload =
                    new ServiceBusMessage(
                        JsonSerializer.Serialize(
                            message,
                            new JsonSerializerOptions
                            {
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                PropertyNamingPolicy = jsonNamingPolicy
                            }));
                payload.Subject = topic;
                payload.TimeToLive = new TimeSpan(12, 0, 0);

                if (!messageBatch.TryAddMessage(payload))
                    throw new Exception($"Unable to add message to batch, batch full");

                await clientSender.SendMessagesAsync(messageBatch);
            }
            finally
            {
                await clientSender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}