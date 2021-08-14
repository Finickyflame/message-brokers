using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace MessageBrokers.Extending
{
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public interface IProducerOptions<TMessage> where TMessage : IMessage
    {
        JsonSerializerOptions SerializerOptions { get; }
    }
}