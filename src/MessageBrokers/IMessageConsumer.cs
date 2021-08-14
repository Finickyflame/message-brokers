using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers
{
    public interface IMessageConsumer
    {
        Task<TMessage> ConsumeAsync<TMessage>(CancellationToken cancellationToken = default) where TMessage : IMessage, new();
        
        Task CommitAsync<TMessage>(TMessage message) where TMessage : IMessage;
    }
}