using System.Threading.Tasks;

namespace MessageBrokers.Extending
{
    public interface IMessageProducer<in TMessage> where TMessage : IMessage
    {
        Task PublishAsync(TMessage message);
    }
}