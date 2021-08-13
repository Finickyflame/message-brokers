using Events;
using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    // todo: implements Extending.IMessageProducer and figure out how to not have an infinite resolution loop with the new behavior    
    internal sealed class RelayMessageProducer : MessageProducer
    {
        private readonly IEventDispatcher _eventDispatcher;

        /// <param name="messageProducer">Must be the <see cref="InMemoryMessageProducer"/> otherwise we will end in an infinite resolution loop.</param>
        /// <param name="eventDispatcher">Must be the <see cref="InMemoryEventDispatcher"/> otherwise we will end in an infinite resolution loop.</param>
        public RelayMessageProducer(InMemoryMessageProducer messageProducer, InMemoryEventDispatcher eventDispatcher)
            : base(messageProducer)
        {
            this._eventDispatcher = eventDispatcher;
        }

        public override async Task PublishAsync<TMessage>(TMessage message)
        {
            await this._eventDispatcher.PublishAsync((dynamic)message.Value);
            await base.PublishAsync(message);
        }
    }
}