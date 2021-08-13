using Confluent.Kafka;
using Events;
using MessageBrokers.Extending;
using MessageBrokers.Kafka.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Json;

namespace MessageBrokers.Kafka
{
    internal class KafkaMessageConverter<TMessage> : IConsumerMessageConverter<ConsumeResult<string, string>, TMessage>, IProducerMessageConverter<Message<string?, string>, TMessage>
        where TMessage : IMessage, new()
    {
        private readonly KafkaConsumerOptions<TMessage> _consumerOptions;
        private readonly KafkaProducerOptions<TMessage> _producerOptions;
        private readonly Type _eventType;

        public KafkaMessageConverter(IOptions<KafkaConsumerOptions<TMessage>> consumerOptions, IOptions<KafkaProducerOptions<TMessage>> producerOptions)
        {
            this._consumerOptions = consumerOptions.Value;
            this._producerOptions = producerOptions.Value;
            this._eventType = typeof(TMessage).GetGenericArguments().First();
        }

        public TMessage ConvertMessage(ConsumeResult<string, string> consumeResult)
        {
            var value = (IEvent)JsonSerializer.Deserialize(consumeResult.Message.Value, this._eventType, this._consumerOptions.SerializerOptions)!;
            return new TMessage
            {
                Key = consumeResult.Message.Key,
                CreatedOn = consumeResult.Message.Timestamp.UtcDateTime,
                Value = value
            };
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