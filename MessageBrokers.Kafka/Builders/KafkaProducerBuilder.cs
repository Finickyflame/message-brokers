using Confluent.Kafka;
using Events;
using MessageBrokers.Kafka.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace MessageBrokers.Kafka
{
    public class KafkaProducerBuilder : Extending.ProducerBuilder<ProducerConfig, KafkaProducerBuilder>
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
                .AddProducerOptions((producerConfig, serializerOptions) => new KafkaProducerOptions<TMessage>(topic, producerConfig, serializerOptions), configure)
                .AddMessageProducer<TMessage, KafkaMessageProducer<TMessage>>();
        }
    }
}