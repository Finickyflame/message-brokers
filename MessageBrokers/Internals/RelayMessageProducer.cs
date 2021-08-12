using Events.Internals;
using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    internal sealed class RelayMessageProducer : IMessageProducer
    {
        private readonly IInternalEventDispatcher _eventDispatcher;
        private readonly IInternalMessageProducer _messageProducer;

        public RelayMessageProducer(IInternalEventDispatcher eventDispatcher, IInternalMessageProducer messageProducer)
        {
            this._eventDispatcher = eventDispatcher;
            this._messageProducer = messageProducer;
        }

        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            await this._messageProducer.PublishAsync(message);
            await this._eventDispatcher.PublishAsync(message.Value.GetType(), message.Value);
        }
    }
}