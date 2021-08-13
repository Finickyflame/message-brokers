using ActiveMQ.Artemis.Client;
using MessageBrokers.Extending;
using System.Text.Json;

namespace MessageBrokers.Artemis.Configurations
{
    public class ArtemisConsumerOptions<TMessage> : IConsumerOptions<TMessage> 
        where TMessage : IMessage
    {
        public ArtemisConsumerOptions(string address, string queue, ConsumerConfiguration configuration, JsonSerializerOptions serializerOptions)
        {
            this.Configuration = new ConsumerConfiguration
            {
                Address = address,
                Queue = queue,
                Credit = configuration.Credit,
                FilterExpression = configuration.FilterExpression,
                RoutingType = configuration.RoutingType,
                NoLocalFilter = configuration.NoLocalFilter
            };
            this.SerializerOptions = new JsonSerializerOptions(serializerOptions);
        }

        public ConsumerConfiguration Configuration { get; }


        public JsonSerializerOptions SerializerOptions { get; }
    }
}