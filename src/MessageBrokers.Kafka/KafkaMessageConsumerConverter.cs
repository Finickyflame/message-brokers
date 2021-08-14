using Confluent.Kafka;
using CQRS.Events;
using MessageBrokers.Extending;
using MessageBrokers.Kafka.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Json;

namespace MessageBrokers.Kafka
{
    internal class KafkaMessageConsumerConverter<TMessage> : IMessageConsumerConverter<ConsumeResult<string, string>, TMessage>
        where TMessage : IMessage, new()
    {
        private readonly KafkaConsumerOptions<TMessage> _consumerOptions;
        private readonly Type _eventType;

        public KafkaMessageConsumerConverter(IOptions<KafkaConsumerOptions<TMessage>> consumerOptions)
        {
            this._consumerOptions = consumerOptions.Value;
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
    }
}