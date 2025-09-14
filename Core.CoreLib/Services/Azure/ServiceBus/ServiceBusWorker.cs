
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.CoreLib.Services.Azure.ServiceBus
{
    public class ServiceBusWorker : IHostedService, IDisposable
    {
        private readonly ILogger<ServiceBusWorker> _logger;
        private readonly IServiceBusTopicSubscription _serviceBusTopicSubscription;

        public ServiceBusWorker(
            IServiceBusTopicSubscription serviceBusTopicSubscription,
            ILogger<ServiceBusWorker> logger)
        {
            _serviceBusTopicSubscription = serviceBusTopicSubscription;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Starting the service bus queue consumer and the subscription");
            await _serviceBusTopicSubscription.HandleMessagesAsync().ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Stopping the service bus queue consumer and the subscription");
            await _serviceBusTopicSubscription.CloseSubscriptionAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (disposing)
                await _serviceBusTopicSubscription.DisposeAsync().ConfigureAwait(false);
        }
    }
}
