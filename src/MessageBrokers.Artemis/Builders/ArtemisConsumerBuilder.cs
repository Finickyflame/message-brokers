using ActiveMQ.Artemis.Client;
using CQRS.Events;
using MessageBrokers.Artemis.Configurations;
using MessageBrokers.Extending;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MessageBrokers.Artemis.Builders
{
    public sealed class ArtemisConsumerBuilder : ConsumerBuilder<ConsumerConfiguration, ArtemisConsumerBuilder>
    {
        public ArtemisConsumerBuilder(IServiceCollection services)
            : base(services)
        {
        }

        /// <summary>
        /// Register an Event to be available with <see cref="IEventDispatcher.PublishAsync{TEvent}"/>.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="queue"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public ArtemisConsumerBuilder AddEvent<TEvent>(string address, string queue, Action<ArtemisConsumerOptions<Message<TEvent>>>? configure = null)
            where TEvent : IEvent
        {
            return this.AddMessage(address, queue, configure);
        }

        /// <summary>
        /// Register a Message to be available with <see cref="MessageBrokers.IMessageProducer.PublishAsync{TMessage}"/>.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="queue"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public ArtemisConsumerBuilder AddMessage<TMessage>(string address, string queue, Action<ArtemisConsumerOptions<TMessage>>? configure = null)
            where TMessage : class, IMessage, new()
        {
            return this
                .AddConsumerOptions<TMessage, ArtemisConsumerOptions<TMessage>>((configuration, serializerOptions)
                    => new ArtemisConsumerOptions<TMessage>(address, queue, configuration, serializerOptions), configure)
                .AddMessageConsumer<TMessage, ArtemisMessageConsumer<TMessage>>();
        }
    }
}