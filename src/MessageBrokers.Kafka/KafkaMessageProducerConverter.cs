using Confluent.Kafka;
using MessageBrokers.Extending;
using MessageBrokers.Kafka.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Json;

namespace MessageBrokers.Kafka
{
    internal class KafkaMessageProducerConverter<TMessage> : IMessageProducerConverter<Message<string?, string>, TMessage>
        where TMessage : IMessage
    {
        private readonly KafkaProducerOptions<TMessage> _producerOptions;
        private readonly Type _eventType;

        public KafkaMessageProducerConverter(IOptions<KafkaProducerOptions<TMessage>> producerOptions)
        {
            this._producerOptions = producerOptions.Value;
            this._eventType = typeof(TMessage).GetGenericArguments().First();
        }

        public Message<string?, string> ConvertMessage(TMessage message)
        {
            string value = JsonSerializer.Serialize(message.Value, this._eventType, this._producerOptions.SerializerOptions);
            Timestamp timeStamp = message.CreatedOn.HasValue ? new Timestamp(message.CreatedOn.Value) : Timestamp.Default;
            return new Message<string?, string>
            {
                Key = message.Key,
                Timestamp = timeStamp,
                Value = value
            };
        }
    }
}