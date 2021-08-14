using ActiveMQ.Artemis.Client;
using MessageBrokers.Artemis.Configurations;
using MessageBrokers.Extending;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Json;

namespace MessageBrokers.Artemis
{
    internal class ArtemisMessageProducerConverter<TMessage> : IMessageProducerConverter<Message, TMessage>
        where TMessage : IMessage
    {
        private readonly ArtemisProducerOptions<TMessage> _producerOptions;
        private readonly Type _eventType;

        public ArtemisMessageProducerConverter(IOptions<ArtemisProducerOptions<TMessage>> producerOptions)
        {
            this._producerOptions = producerOptions.Value;
            this._eventType = typeof(TMessage).GetGenericArguments().First();
        }

        public Message ConvertMessage(TMessage message)
        {
            string value = JsonSerializer.Serialize(message.Value, this._eventType, this._producerOptions.SerializerOptions);
            var result = new Message(value)
            {
                CreationTime = message.CreatedOn?.DateTime
            };
            result.SetMessageId(message.Key);
            return result;
        }
    }
}