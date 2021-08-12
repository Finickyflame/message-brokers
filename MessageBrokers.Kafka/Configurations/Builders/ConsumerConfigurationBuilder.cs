using Confluent.Kafka;
using Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace MessageBrokers.Kafka.Configurations
{
    public sealed class ConsumerConfigurationBuilder
    {
        public ConsumerConfigurationBuilder(IServiceCollection services)
        {
            this.Services = services;
            this.SerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public IServiceCollection Services { get; }

        public JsonSerializerOptions SerializerOptions { get; }

        /// <summary>
        /// Register an Event to be available with <see cref="MessageConsumerExtensions.ConsumeEventAsync{TEvent}"/>.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="groupId"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public ConsumerConfigurationBuilder AddEvent<TEvent>(string topic, string groupId, Action<ConsumerConfiguration<Message<TEvent>>>? configure = null) where TEvent : IEvent
            => this.AddMessage(topic, groupId, configure);

        /// <summary>
        /// Register a Message to be available with <see cref="IMessageConsumer.ConsumeAsync{TMessage}"/>.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="groupId"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public ConsumerConfigurationBuilder AddMessage<TMessage>(string topic, string groupId, Action<ConsumerConfiguration<TMessage>>? configure = null) where TMessage : IMessage, new()
        {
            this.Services.AddSingleton(provider =>
                {
                    var configuration = new ConsumerConfiguration<TMessage>(
                        provider.GetRequiredService<ConsumerConfig>(),
                        this.SerializerOptions,
                        topic, groupId);
                    configure?.Invoke(configuration);
                    return configuration;
                })
                .AddBackgroundMessageService<TMessage>();
            return this;
        }

        public ConsumerConfigurationBuilder ConfigureSerializerOptions(Action<JsonSerializerOptions> configure)
        {
            configure(this.SerializerOptions);
            return this;
        }
    }
}