using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers
{
    public abstract class MessageConsumer : IMessageConsumer
    {
        private readonly IMessageConsumer _base;

        protected MessageConsumer(IMessageConsumer @base)
        {
            this._base = @base;
        }

        public virtual Task<TMessage> ConsumeAsync<TMessage>(CancellationToken cancellationToken = default) where TMessage : IMessage, new() 
            => this._base.ConsumeAsync<TMessage>(cancellationToken);

        public virtual Task CommitAsync<TMessage>(TMessage message) where TMessage : IMessage 
            => this._base.CommitAsync(message);
    }
}