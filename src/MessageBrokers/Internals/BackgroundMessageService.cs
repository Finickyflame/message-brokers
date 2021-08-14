using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    internal sealed class BackgroundMessageService<TMessage> 
        where TMessage : IMessage, new() 
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly IMessageProducer _messageProducer;

        public BackgroundMessageService(IMessageConsumer messageConsumer, IMessageProducer messageProducer)
        {
            this._messageConsumer = messageConsumer;
            this._messageProducer = messageProducer;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // This will hang the background service.
                // It will wait for the next available message.
                var message = await this._messageConsumer.ConsumeAsync<TMessage>(stoppingToken);

                await this._messageProducer.PublishAsync(message);
                await this._messageConsumer.CommitAsync(message);
            }
        }
    }
}