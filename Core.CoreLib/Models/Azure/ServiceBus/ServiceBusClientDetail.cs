using Azure.Messaging.ServiceBus;

namespace Core.CoreLib.Models.Azure.ServiceBus
{
    public class ServiceBusClientDetail
    {
        public ServiceBusClient ServiceBusClient { get; set; }

        public string ConnectionString { get; set; }

        public string TopicName { get; set; }

        public string Subscription { get; set; }
    }
}