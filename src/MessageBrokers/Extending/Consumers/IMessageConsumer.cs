using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers.Extending
{
    public interface IMessageConsumer<TMessage> where TMessage : IMessage
    {
        Task<TMessage> ConsumeAsync(CancellationToken cancellationToken = default);

        Task CommitAsync(TMessage message);
    }
}