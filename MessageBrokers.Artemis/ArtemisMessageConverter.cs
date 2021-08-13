using ActiveMQ.Artemis.Client;
using Events;
using MessageBrokers.Artemis.Configurations;
using MessageBrokers.Extending;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Json;

namespace MessageBrokers.Artemis
{
    internal class ArtemisMessageConverter<TMessage> : IConsumerMessageConverter<Message, TMessage>, IProducerMessageConverter<Message, TMessage>
        where TMessage : IMessage, new()
    {
        private readonly ArtemisConsumerOptions<TMessage> _consumerOptions;
        private readonly ArtemisProducerOptions<TMessage> _producerOptions;
        private readonly Type _eventType;

        public ArtemisMessageConverter(IOptions<ArtemisProducerOptions<TMessage>> producerOptions, IOptions<ArtemisConsumerOptions<TMessage>> consumerOptions)
        {
            this._consumerOptions = consumerOptions.Value;
            this._producerOptions = producerOptions.Value;
            this._eventType = typeof(TMessage).GetGenericArguments().First();
        }
        
        public TMessage ConvertMessage(Message message)
        {
            var value = (IEvent)JsonSerializer.Deserialize(message.GetBody<string>(), this._eventType, this._consumerOptions.SerializerOptions)!;
            return new TMessage
            {
                Key = message.GetMessageId<string>(),
                Value = value,
                CreatedOn = message.CreationTime
            };
        }

        public Message ConvertMessage(TMessage message)
        {
            string value = JsonSerializer.Serialize(message.Value, this._eventType, this._producerOptions.SerializerOptions)!;
            var result = new Message(value)
            {
                CreationTime = message.CreatedOn?.DateTime
            };
            result.SetMessageId(message.Key);
            return result;
        }
    }
}