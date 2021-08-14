using Confluent.Kafka;
using MessageBrokers.Extending;
using MessageBrokers.Kafka.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace MessageBrokers.Kafka
{
    internal sealed class KafkaMessageProducer<TMessage> : IMessageProducer<TMessage>, IDisposable
        where TMessage : IMessage, new()
    {
        private readonly KafkaProducerOptions<TMessage> _options;
        private readonly KafkaMessageProducerConverter<TMessage> _converter;
        private readonly IProducer<string?, string> _producer;

        public KafkaMessageProducer(IOptions<KafkaProducerOptions<TMessage>> options, KafkaMessageProducerConverter<TMessage> converter)
        {
            this._options = options.Value;
            this._converter = converter;
            this._producer = CreateProducer(this._options);
        }

        public async Task PublishAsync(TMessage message)
        {
            await this._producer.ProduceAsync(this._options.Topic, this._converter.ConvertMessage(message));
        }

        private static IProducer<string?, string> CreateProducer(KafkaProducerOptions<TMessage> options)
        {
            return new Confluent.Kafka.ProducerBuilder<string?, string>(options.KafkaConfig).Build();
        }

        public void Dispose()
        {
            this._producer.Flush();
            this._producer.Dispose();
        }
    }
}