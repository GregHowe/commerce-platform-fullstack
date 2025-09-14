using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Core.Backend.Services.KeyVault;
using Core.CoreLib.Models.Azure.ServiceBus;
using Core.CoreLib.Models.Settings;
using Core.CoreLib.Services.Azure.ServiceBus.Processors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.CoreLib.Services.Azure.ServiceBus
{
    public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
    {
       
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private List<ServiceBusProcessor> _processors = new List<ServiceBusProcessor>();
        private List<ServiceBusClientDetail> _clients = new List<ServiceBusClientDetail>();
        private IServiceScope _scope;

        public ServiceBusTopicSubscription(
            ILogger<ServiceBusTopicSubscription> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;

            _scope = _serviceScopeFactory.CreateScope();
        }

        protected IEnumerable<IServiceBusMessageProcessor> GetProcessors(IServiceScope scope, string subject)
        {
            // This works, seems like could be written better, too many ??
            // Get all the IServiceBusMessageProcessor or only that match subject
            return
                (string.IsNullOrWhiteSpace(subject) ?
                    (scope.ServiceProvider.GetService(typeof(IEnumerable<IServiceBusMessageProcessor>)) as IEnumerable<IServiceBusMessageProcessor> ?? 
                    new List<IServiceBusMessageProcessor>())
                    .ToList() :
                    (scope.ServiceProvider.GetService(typeof(IEnumerable<IServiceBusMessageProcessor>)) as IEnumerable<IServiceBusMessageProcessor> ?? 
                    new List<IServiceBusMessageProcessor>())
                    .Where(w => w.Subject == subject)
                    .ToList()) ??
                    new List<IServiceBusMessageProcessor>();
        }
        
        public async Task HandleMessagesAsync()
        {
            var _serviceBusProcessorOptions =
                new ServiceBusProcessorOptions
                {
                    MaxConcurrentCalls = 10,
                    AutoCompleteMessages = false,
                };

            foreach (var processor in GetProcessors(_scope, string.Empty))
            {
                var client =
                    new ServiceBusClient(processor.ConnectionString);

                _clients.Add(
                    new ServiceBusClientDetail() 
                    { 
                        ServiceBusClient = client, 
                        ConnectionString = processor.ConnectionString, 
                        TopicName = processor.Topic, 
                        Subscription = processor.Subscription
                    });

                var sbProcessor =
                    client.CreateProcessor(processor.Topic, processor.Subscription, _serviceBusProcessorOptions);
                _processors.Add(sbProcessor);

                sbProcessor.ProcessMessageAsync += ProcessMessagesAsync;
                sbProcessor.ProcessErrorAsync += ProcessErrorAsync;

                await sbProcessor.StartProcessingAsync().ConfigureAwait(false);
            }
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            // Get the processors that can process this subject
            var processors =
                GetProcessors(_scope, args?.Message?.Subject ?? string.Empty);

            // Process
            foreach (var processor in processors)
                await processor.ProcessMessageAsync(args).ConfigureAwait(false);

            // If we don't have a processor setup to process this message, don't complete.
            if (processors.Count() > 0)
                await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
            else
            {
                await args.DeadLetterMessageAsync(args.Message, "No listener for messages with this Subject").ConfigureAwait(false);
                _logger.LogError($"Unexpected message received.  Subject: {args?.Message?.Subject ?? string.Empty}. Message Id: {args?.Message?.MessageId}");
            }
        }
        
        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Message handler encountered an exception");
            _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
            _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
            _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var processor in _processors)
                await processor.DisposeAsync().ConfigureAwait(false);

            foreach (var client in _clients)
                await client.ServiceBusClient.DisposeAsync().ConfigureAwait(false);
        }

        public async Task CloseSubscriptionAsync()
        {
            foreach (var processor in _processors)
                await processor.CloseAsync().ConfigureAwait(false);
        }

        public async Task<Dictionary<string, int>> GetTopicDeadLetterCount()
        {
            var result = new Dictionary<string, int>();
            
            foreach (var client in _clients)
            {
                var adminClient = new ServiceBusAdministrationClient(client.ConnectionString);
                var subscriptionProps = await adminClient.GetSubscriptionRuntimePropertiesAsync(client.TopicName, client.Subscription);
                int count = (int)subscriptionProps.Value.DeadLetterMessageCount;

                result.Add($"{client.ServiceBusClient.FullyQualifiedNamespace}:{client.TopicName}:{client.Subscription}", count);
            }

            return result;
        }
    }
}