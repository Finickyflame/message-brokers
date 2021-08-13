using Bogus;
using Events;
using KafkaProduceConsole.Events;
using KafkaProduceConsole.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KafkaProduceConsole.Tasks
{
    public class ProduceOrdersCreatedTask : IApplicationTask
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly Faker<OrderCreatedEvent> _eventFactory;
        private readonly ILogger<ProduceOrdersCreatedTask> _logger;
        private readonly TaskOptions _taskOptions;

        public ProduceOrdersCreatedTask(
            IEventDispatcher eventDispatcher, 
            Faker<OrderCreatedEvent> eventFactory,
            IOptions<TaskOptions> taskOptions, 
            ILogger<ProduceOrdersCreatedTask> logger)
        {
            this._eventDispatcher = eventDispatcher;
            this._eventFactory = eventFactory;
            this._taskOptions = taskOptions.Value;
            this._logger = logger;
        }

        public string Name => nameof(ProduceOrdersCreatedTask);

        public async Task RunAsync()
        {
            foreach (OrderCreatedEvent @event in this._eventFactory.GenerateLazy(this._taskOptions.ProduceCount))
            {
                await this._eventDispatcher.PublishAsync(@event);
                this._logger.LogInformation("Event produced: {Event}", @event);
            }
        }
    }
}