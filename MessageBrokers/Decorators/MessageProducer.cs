using System.Threading.Tasks;

namespace MessageBrokers
{
    public abstract class MessageProducer : IMessageProducer
    {
        private readonly IMessageProducer _base;

        protected MessageProducer(IMessageProducer @base)
        {
            this._base = @base;
        }

        public virtual Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage => this._base.PublishAsync(message);
    }
}