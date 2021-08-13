using System.Text.Json;

namespace MessageBrokers.Extending
{
    public interface IConsumerOptions<TMessage> where TMessage : IMessage
    {
        JsonSerializerOptions SerializerOptions { get; }
    }
}