using Events;
using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    internal sealed class ToMessageEventDispatcher : IEventDispatcher
    {
        private readonly IMessageProducer _messageProducer;

        public ToMessageEventDispatcher(IMessageProducer messageProducer)
        {
            this._messageProducer = messageProducer;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            await this._messageProducer.PublishAsync(new Message<TEvent>(@event));
        }
    }
}