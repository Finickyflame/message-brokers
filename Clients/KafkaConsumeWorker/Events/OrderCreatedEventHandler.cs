using Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KafkaConsumeWorker.Events
{
    public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedEventHandler> _logger;

        public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
        {
            this._logger = logger;
        }

        public Task HandleAsync(OrderCreatedEvent @event)
        {
            this._logger.LogInformation("Event received: {Event}", @event);
            return Task.CompletedTask;
        }
    }
}