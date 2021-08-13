using Confluent.Kafka;
using System.Text.Json;

namespace MessageBrokers.Kafka.Configurations
{
    // ReSharper disable once UnusedTypeParameter
    public class KafkaProducerOptions<TMessage> where TMessage : IMessage
    {
        internal KafkaProducerOptions(string topic, ProducerConfig producerConfig, JsonSerializerOptions serializerOptions)
        {
            this.Topic = topic;
            this.KafkaConfig = new ProducerConfig(producerConfig);
            this.SerializerOptions = new JsonSerializerOptions(serializerOptions);
        }
        
        public ProducerConfig KafkaConfig { get; }
        
        public string Topic { get; }
        
        public JsonSerializerOptions SerializerOptions { get; }
    }
}