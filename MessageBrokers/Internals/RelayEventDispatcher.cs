using Events;
using Events.Internals;
using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    internal sealed class RelayEventDispatcher : IEventDispatcher
    {
        private readonly IInternalEventDispatcher _eventDispatcher;
        private readonly IInternalMessageProducer _messageProducer;

        public RelayEventDispatcher(IInternalEventDispatcher eventDispatcher, IInternalMessageProducer messageProducer)
        {
            this._eventDispatcher = eventDispatcher;
            this._messageProducer = messageProducer;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            await this._messageProducer.PublishAsync(new Message<TEvent>(@event));
            await this._eventDispatcher.PublishAsync(@event);
        }
    }
}