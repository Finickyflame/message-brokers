using Confluent.Kafka;
using Events;
using MessageBrokers.Kafka.Configurations;
using System.Linq;
using System.Text.Json;

namespace MessageBrokers.Kafka
{
    internal class KafkaMessageConverter
    {
        public TMessage ConvertMessage<TMessage>(ConsumeResult<string,string> consumeResult, ConsumerConfiguration<TMessage> configuration) 
            where TMessage : IMessage, new()
        {
            var value = (IEvent)JsonSerializer.Deserialize(consumeResult.Message.Value, typeof(TMessage).GetGenericArguments().First(), configuration.SerializerOptions)!;
            return new TMessage
            {
                Key = consumeResult.Message.Key,
                CreatedOn = consumeResult.Message.Timestamp.UtcDateTime,
                Value = value
            };
        }
        
        public Message<string, string> ConvertMessage<TMessage>(TMessage message, ProducerConfiguration<TMessage> configuration)
            where TMessage : IMessage
        {
            string value = JsonSerializer.Serialize(message.Value, typeof(TMessage).GetGenericArguments().First(), configuration.SerializerOptions)!;
            return new Message<string, string>
            {
                Key = message.Key,
                Timestamp = new Timestamp(message.CreatedOn),
                Value = value
            };
        }
    }
}