using Confluent.Kafka;
using MessageBrokers.Internals;
using MessageBrokers.Kafka.Configurations;
using System.Threading.Tasks;

namespace MessageBrokers.Kafka
{
    internal sealed class KafkaMessageProducer : IInternalMessageProducer
    {
        private readonly ProducerConfigurationProvider _configurationProvider;
        private readonly KafkaMessageConverter _messageConverter;

        public KafkaMessageProducer(ProducerConfigurationProvider configurationProvider, KafkaMessageConverter messageConverter)
        {
            this._configurationProvider = configurationProvider;
            this._messageConverter = messageConverter;
        }

        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            if (this._configurationProvider.TryGetConfiguration(out ProducerConfiguration<TMessage>? configuration))
            {
                using IProducer<string, string> producer = new ProducerBuilder<string, string>(configuration.KafkaConfig).Build();
                await producer.ProduceAsync(configuration.Topic, this._messageConverter.ConvertMessage(message, configuration));
            }
        }
    }
}