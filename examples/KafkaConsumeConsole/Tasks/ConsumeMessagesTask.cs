using KafkaConsumeConsole.Events;
using KafkaConsumeConsole.Options;
using MessageBrokers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KafkaConsumeConsole.Tasks
{
    public class ConsumeMessagesTask : IApplicationTask
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly ILogger<ConsumeMessagesTask> _logger;
        private readonly TaskOptions _taskOptions;

        public ConsumeMessagesTask(IMessageConsumer messageConsumer, IOptions<TaskOptions> taskOptions, ILogger<ConsumeMessagesTask> logger)
        {
            this._messageConsumer = messageConsumer;
            this._taskOptions = taskOptions.Value;
            this._logger = logger;
        }

        public string Name => nameof(ConsumeMessagesTask);

        public async Task RunAsync()
        {
            IAsyncEnumerable<IMessage> orderCreated = this._messageConsumer.ConsumeAllEventsAsync<OrderCreatedEvent>(this._taskOptions.ConsumeWaitDuration);
            IAsyncEnumerable<IMessage> orderCanceled = this._messageConsumer.ConsumeAllEventsAsync<OrderCanceledEvent>(this._taskOptions.ConsumeWaitDuration);
            IAsyncEnumerable<IMessage> messages = orderCreated.Concat(orderCanceled);
            
            await foreach (var message in messages)
            {
                this._logger.LogInformation("Event received: {Event}", message.Value);
                if (this._taskOptions.AllowCommit)
                {
                    await this._messageConsumer.CommitAsync(message);
                    this._logger.LogInformation("Event committed");
                }
            }
        }
    }
}