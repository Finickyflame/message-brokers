using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    internal interface IInternalMessageConsumer : IMessageConsumer
    {
        // Task<IMessage> ConsumeAsync(Type messageType, CancellationToken cancellationToken = default);

        Task CommitAsync<TMessage>(TMessage message) where TMessage : IMessage;
    }
}