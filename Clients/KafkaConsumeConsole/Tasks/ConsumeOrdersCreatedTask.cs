using KafkaConsumeConsole.Events;
using KafkaConsumeConsole.Options;
using MessageBrokers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            await foreach (var message in this._messageConsumer.ConsumeAllAsync<Message<OrderCreatedEvent>>(this._taskOptions.ConsumeWaitDuration))
            {
                this._logger.LogInformation("Event received: {Event}", message.Value);
                if (this._taskOptions.AllowCommit)
                {
                    await this._messageConsumer.CommitAsync(message);
                }
            }
        }
    }
}