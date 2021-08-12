using Confluent.Kafka;
using System.Text.Json;

namespace MessageBrokers.Kafka.Configurations
{
    public class ProducerConfiguration<TMessage> where TMessage : IMessage
    {
        internal ProducerConfiguration(ClientConfig clientConfig, JsonSerializerOptions serializerOptions, string topic)
        {
            this.KafkaConfig = new ProducerConfig(clientConfig);
            this.SerializerOptions = serializerOptions;
            this.Topic = topic;
        }
        
        public ProducerConfig KafkaConfig { get; }
        
        public string Topic { get; }
        
        public JsonSerializerOptions SerializerOptions { get; }
    }
}