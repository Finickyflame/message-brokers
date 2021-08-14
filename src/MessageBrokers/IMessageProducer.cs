using System.Threading.Tasks;

namespace MessageBrokers
{
    public interface IMessageProducer
    {
        Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;
    }
}