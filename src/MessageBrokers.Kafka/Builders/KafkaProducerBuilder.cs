using Confluent.Kafka;
using CQRS.Events;
using MessageBrokers.Kafka.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MessageBrokers.Kafka
{
    public sealed class KafkaProducerBuilder : Extending.ProducerBuilder<ProducerConfig, KafkaProducerBuilder>
    {
        public KafkaProducerBuilder(IServiceCollection services)
            : base(services)
        {
        }

        /// <summary>
        /// Register an Event to be available with <see cref="IEventDispatcher.PublishAsync{TEvent}"/>.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public KafkaProducerBuilder AddEvent<TEvent>(string topic, Action<KafkaProducerOptions<Message<TEvent>>>? configure = null)
            where TEvent : IEvent
        {
            return this.AddMessage(topic, configure);
        }

        /// <summary>
        /// Register a Message to be available with <see cref="MessageBrokers.IMessageProducer.PublishAsync{TMessage}"/>.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public KafkaProducerBuilder AddMessage<TMessage>(string topic, Action<KafkaProducerOptions<TMessage>>? configure = null)
            where TMessage : class, IMessage, new()
        {
            return this
                .AddProducerOptions<TMessage, KafkaProducerOptions<TMessage>>((producerConfig, serializerOptions)
                    => new KafkaProducerOptions<TMessage>(topic, producerConfig, serializerOptions), configure)
                .AddMessageProducer<TMessage, KafkaMessageProducer<TMessage>>();
        }
    }
}