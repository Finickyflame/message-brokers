using CQRS.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KafkaConsumeWorker.Events
{
    public class OrderCanceledEventHandler : IEventHandler<OrderCanceledEvent>
    {
        private readonly ILogger<OrderCanceledEventHandler> _logger;

        public OrderCanceledEventHandler(ILogger<OrderCanceledEventHandler> logger)
        {
            this._logger = logger;
        }

        public Task HandleAsync(OrderCanceledEvent @event)
        {
            this._logger.LogInformation("Event received: {Event}", @event);
            return Task.CompletedTask;
        }
    }
}