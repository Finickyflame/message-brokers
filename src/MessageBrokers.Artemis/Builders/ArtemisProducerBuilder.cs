using ActiveMQ.Artemis.Client;
using CQRS.Events;
using MessageBrokers.Artemis.Configurations;
using MessageBrokers.Extending;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MessageBrokers.Artemis.Builders
{
    public sealed class ArtemisProducerBuilder : ProducerBuilder<ProducerConfiguration, ArtemisProducerBuilder>
    {
        public ArtemisProducerBuilder(IServiceCollection services)
            : base(services)
        {
        }

        /// <summary>
        /// Register an Event to be available with <see cref="IEventDispatcher.PublishAsync{TEvent}"/>.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public ArtemisProducerBuilder AddEvent<TEvent>(string address, Action<ArtemisProducerOptions<Message<TEvent>>>? configure = null)
            where TEvent : IEvent
        {
            return this.AddMessage(address, configure);
        }

        /// <summary>
        /// Register a Message to be available with <see cref="MessageBrokers.IMessageProducer.PublishAsync{TMessage}"/>.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public ArtemisProducerBuilder AddMessage<TMessage>(string address, Action<ArtemisProducerOptions<TMessage>>? configure = null)
            where TMessage : class, IMessage, new()
        {
            return this
                .AddProducerOptions<TMessage, ArtemisProducerOptions<TMessage>>((configuration, serializerOptions) => 
                    new ArtemisProducerOptions<TMessage>(address, configuration, serializerOptions), configure)
                .AddMessageProducer<TMessage, ArtemisMessageProducer<TMessage>>();
        }
    }
}