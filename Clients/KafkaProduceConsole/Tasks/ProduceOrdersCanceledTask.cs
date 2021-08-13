using Bogus;
using Events;
using KafkaProduceConsole.Events;
using KafkaProduceConsole.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace KafkaProduceConsole.Tasks
{
    public class ProduceOrdersCanceledTask : IApplicationTask
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly Faker<OrderCanceledEvent> _eventFactory;
        private readonly ILogger<ProduceOrdersCanceledTask> _logger;
        private readonly TaskOptions _taskOptions;

        public ProduceOrdersCanceledTask(
            IEventDispatcher eventDispatcher, 
            Faker<OrderCanceledEvent> eventFactory,
            IOptions<TaskOptions> taskOptions, 
            ILogger<ProduceOrdersCanceledTask> logger)
        {
            this._eventDispatcher = eventDispatcher;
            this._eventFactory = eventFactory;
            this._taskOptions = taskOptions.Value;
            this._logger = logger;
        }

        public string Name => nameof(ProduceOrdersCanceledTask);

        public async Task RunAsync()
        {
            foreach (OrderCanceledEvent @event in this._eventFactory.GenerateLazy(this._taskOptions.ProduceCount))
            {
                await this._eventDispatcher.PublishAsync(@event);
                this._logger.LogInformation("Event produced: {Event}", @event);
            }
        }
    }
}