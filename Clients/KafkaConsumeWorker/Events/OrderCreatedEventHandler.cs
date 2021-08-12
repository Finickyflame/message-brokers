using Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KafkaConsumeWorker.Events
{
    public class ImpairedChangedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly ILogger<ImpairedChangedEventHandler> _logger;

        public ImpairedChangedEventHandler(ILogger<ImpairedChangedEventHandler> logger)
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