using ActiveMQ.Artemis.Client;
using MessageBrokers.Extending;
using System.Text.Json;

namespace MessageBrokers.Artemis.Configurations
{
    public class ArtemisProducerOptions<TMessage> : IProducerOptions<TMessage> where TMessage : IMessage
    {
        public ArtemisProducerOptions(string address, ProducerConfiguration configuration, JsonSerializerOptions serializerOptions)
        {
            this.Configuration = new ProducerConfiguration
            {
                Address = address,
                MessagePriority = configuration.MessagePriority,
                RoutingType = configuration.RoutingType,
                MessageDurabilityMode = configuration.MessageDurabilityMode,
                MessageIdPolicy = configuration.MessageIdPolicy,
                SetMessageCreationTime = configuration.SetMessageCreationTime
            };
            this.SerializerOptions = new JsonSerializerOptions(serializerOptions);
        }
        
        public ProducerConfiguration Configuration { get; }

        public JsonSerializerOptions SerializerOptions { get; }
    }
}