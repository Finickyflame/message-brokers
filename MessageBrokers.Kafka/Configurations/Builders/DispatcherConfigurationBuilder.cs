using Confluent.Kafka;
using Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace MessageBrokers.Kafka.Configurations
{
    public class DispatcherConfigurationBuilder
    {
        public DispatcherConfigurationBuilder(IServiceCollection services)
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
        /// Register an Event to be available with <see cref="IEventDispatcher.PublishAsync{TEvent}"/>.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public DispatcherConfigurationBuilder AddEvent<TEvent>(string topic, Action<ProducerConfiguration<Message<TEvent>>>? configure = null) 
            where TEvent : IEvent
        {
            return this.AddMessage(topic, configure);
        }

        /// <summary>
        /// Register a Message to be available with <see cref="IMessageProducer.PublishAsync{TMessage}"/>.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public DispatcherConfigurationBuilder AddMessage<TMessage>(string topic, Action<ProducerConfiguration<TMessage>>? configure = null) where TMessage : IMessage
        {
            this.Services.AddSingleton(services =>
            {
                var configuration = new ProducerConfiguration<TMessage>(
                    services.GetRequiredService<ProducerConfig>(),
                    this.SerializerOptions,
                    topic
                );
                configure?.Invoke(configuration);
                return configuration;
            });
            return this;
        }

        public DispatcherConfigurationBuilder ConfigureSerializerOptions(Action<JsonSerializerOptions> configure)
        {
            configure(this.SerializerOptions);
            return this;
        }
    }
}