using System.Threading.Tasks;

namespace MessageBrokers
{
    public interface IMessageHandler<in TMessage> where TMessage : IMessage
    {
        Task HandleAsync(TMessage message);
    }
}