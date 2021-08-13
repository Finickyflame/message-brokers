using Confluent.Kafka;
using Events;
using MessageBrokers.Kafka.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace MessageBrokers.Kafka
{
    public sealed class KafkaConsumerBuilder : Extending.ConsumerBuilder<ConsumerConfig, KafkaConsumerBuilder>
    {
        public KafkaConsumerBuilder(IServiceCollection services)
            : base(services)
        {
            this.Configure(options =>
            {
                options.EnableAutoCommit = false;
                options.AutoOffsetReset = AutoOffsetReset.Earliest;
            });
        }

        /// <summary>
        /// Register an Event to be available with <see cref="MessageConsumerExtensions.ConsumeEventAsync{TEvent}"/>.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="groupId"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public KafkaConsumerBuilder AddEvent<TEvent>(string topic, string groupId, Action<KafkaConsumerOptions<Message<TEvent>>>? configure = null) where TEvent : IEvent
            => this.AddMessage(topic, groupId, configure);

        /// <summary>
        /// Register a Message to be available with <see cref="IMessageConsumer.ConsumeAsync{TMessage}"/>.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="groupId"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public KafkaConsumerBuilder AddMessage<TMessage>(string topic, string groupId, Action<KafkaConsumerOptions<TMessage>>? configure = null) where TMessage : class, IMessage, new()
        {
            return this
                .AddConsumerOptions((consumerConfig, serializerOptions) => new KafkaConsumerOptions<TMessage>(topic, groupId, consumerConfig, serializerOptions), configure)
                .AddMessageConsumer<TMessage, KafkaMessageConsumer<TMessage>>();
        }
    }
}