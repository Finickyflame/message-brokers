using System.Text.Json;

namespace MessageBrokers.Extending
{
    public interface IProducerOptions<TMessage> where TMessage : IMessage
    {
        JsonSerializerOptions SerializerOptions { get; }
    }
}