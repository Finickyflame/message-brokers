using Confluent.Kafka;
using MessageBrokers.Extending;
using MessageBrokers.Kafka.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers.Kafka
{
    internal sealed class KafkaMessageConsumer<TMessage> : IMessageConsumer<TMessage>, IDisposable
        where TMessage : IMessage, new()
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly KafkaMessageCollection _messages;
        private readonly KafkaMessageConsumerConverter<TMessage> _converter;

        public KafkaMessageConsumer(
            IOptions<KafkaConsumerOptions<TMessage>> options,
            KafkaMessageCollection messages,
            KafkaMessageConsumerConverter<TMessage> converter)
        {
            this._consumer = CreateConsumer(options.Value);
            this._messages = messages;
            this._converter = converter;
        }

        public async Task<TMessage> ConsumeAsync(CancellationToken cancellationToken = default) => await Task.Run(() =>
        {
            ConsumeResult<string, string> consumeResult = this._consumer.Consume(cancellationToken);

            TMessage message = this._converter.ConvertMessage(consumeResult);
            this._messages.Add(message, consumeResult);
            return message;
        }, cancellationToken).ConfigureAwait(false);

        public async Task CommitAsync(TMessage message)
        {
            if (this._messages.TryGet(message, out ConsumeResult<string, string>? consumeResult))
            {
                await Task.Run(() => this._consumer.Commit(consumeResult)).ConfigureAwait(false);
            }
        }

        private static IConsumer<string, string> CreateConsumer(KafkaConsumerOptions<TMessage> options)
        {
            IConsumer<string, string>? consumer = new Confluent.Kafka.ConsumerBuilder<string, string>(options.KafkaConfig).Build();
            consumer.Subscribe(options.Topic);

            AssignOffSets(consumer, options.TimeStamp, options.Timeout);
            return consumer;
        }

        private static void AssignOffSets(IConsumer<string, string> consumer, Timestamp? timeStamp, TimeSpan timeout)
        {
            if (!timeStamp.HasValue)
            {
                return;
            }

            IEnumerable<TopicPartitionTimestamp> partitionTimestamps = consumer.Assignment.Select(topic => new TopicPartitionTimestamp(topic, timeStamp.Value));
            consumer.Assign(consumer.OffsetsForTimes(partitionTimestamps, timeout));
        }

        public void Dispose()
        {
            this._consumer.Close();
            this._consumer.Dispose();
        }
    }
}