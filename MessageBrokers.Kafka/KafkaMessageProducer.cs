using Confluent.Kafka;
using MessageBrokers.Internals;
using MessageBrokers.Kafka.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBrokers.Kafka
{
    internal sealed class KafkaMessageProducer : IInternalMessageProducer, IDisposable
    {
        private readonly ProducerConfigurationProvider _configurationProvider;
        private readonly KafkaMessageConverter _messageConverter;
        private readonly Dictionary<Type, IProducer<string, string>> _cachedProducers = new();

        public KafkaMessageProducer(ProducerConfigurationProvider configurationProvider, KafkaMessageConverter messageConverter)
        {
            this._configurationProvider = configurationProvider;
            this._messageConverter = messageConverter;
        }

        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            if (this._configurationProvider.TryGetConfiguration(out ProducerConfiguration<TMessage>? configuration))
            {
                IProducer<string, string> producer = this.CreateProducer(configuration);
                await producer.ProduceAsync(configuration.Topic, this._messageConverter.ConvertMessage(message, configuration));
            }
        }

        private IProducer<string, string> CreateProducer<TMessage>(ProducerConfiguration<TMessage> configuration) where TMessage : IMessage
        {
            if (!this._cachedProducers.TryGetValue(typeof(TMessage), out IProducer<string, string>? producer))
            {
                producer = new ProducerBuilder<string, string>(configuration.KafkaConfig).Build();
                this._cachedProducers.Add(typeof(TMessage), producer);
            }
            return producer;
        }

        public void Dispose()
        {
            foreach (var producer in this._cachedProducers.Values)
            {
                producer.Flush();
                producer.Dispose();
            }
        }
    }
}