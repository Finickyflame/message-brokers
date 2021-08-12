using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    internal class ThrowsUnregisteredMessageConsumer : IMessageConsumer
    {
        public Task<TMessage> ConsumeAsync<TMessage>(CancellationToken cancellationToken = default) where TMessage : IMessage, new()
            => throw new UnregisteredMessageException<TMessage>();

        public Task CommitAsync<TMessage>(TMessage message) where TMessage : IMessage
            => throw new UnregisteredMessageException<TMessage>();
    }
}