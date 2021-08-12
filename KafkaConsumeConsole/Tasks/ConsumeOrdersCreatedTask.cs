using KafkaConsumeConsole.Events;
using KafkaConsumeConsole.Options;
using MessageBrokers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KafkaConsumeConsole.Tasks
{
    public class ConsumeOrdersCreatedTask : IApplicationTask
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly ILogger<ConsumeOrdersCreatedTask> _logger;
        private readonly TaskOptions _taskOptions;

        public ConsumeOrdersCreatedTask(IMessageConsumer messageConsumer, IOptions<TaskOptions> taskOptions, ILogger<ConsumeOrdersCreatedTask> logger)
        {
            this._messageConsumer = messageConsumer;
            this._taskOptions = taskOptions.Value;
            this._logger = logger;
        }

        public string Name => nameof(ConsumeOrdersCreatedTask);

        public async Task RunAsync()
        {
            this._logger.LogInformation("{Task} started", nameof(ConsumeOrdersCreatedTask));
            var watch = Stopwatch.StartNew();
            
            await foreach (var @event in this._messageConsumer.ConsumeAllEventsAsync<OrderCreatedEvent>(this._taskOptions.ConsumeWaitDuration))
            {
                this._logger.LogInformation("Event received: {Event}", @event);
            }
            
            watch.Stop();
            this._logger.LogInformation("{Task} completed ({Duration}ms)", nameof(ConsumeOrdersCreatedTask), watch.ElapsedMilliseconds);
        }
    }
}