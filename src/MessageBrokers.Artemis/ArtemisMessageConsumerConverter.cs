using ActiveMQ.Artemis.Client;
using CQRS.Events;
using MessageBrokers.Artemis.Configurations;
using MessageBrokers.Extending;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Json;

namespace MessageBrokers.Artemis
{
    internal class ArtemisMessageConsumerConverter<TMessage> : IMessageConsumerConverter<Message, TMessage>
        where TMessage : IMessage, new()
    {
        private readonly ArtemisConsumerOptions<TMessage> _consumerOptions;
        private readonly Type _eventType;

        public ArtemisMessageConsumerConverter(IOptions<ArtemisConsumerOptions<TMessage>> consumerOptions)
        {
            this._consumerOptions = consumerOptions.Value;
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
    }
}