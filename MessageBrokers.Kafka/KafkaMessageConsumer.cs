using Confluent.Kafka;
using MessageBrokers.Internals;
using MessageBrokers.Kafka.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers.Kafka
{
    internal sealed class KafkaMessageConsumer : IInternalMessageConsumer, IDisposable
    {
        private readonly ConsumerConfigurationProvider _configurationProvider;
        private readonly KafkaMessageConsumerResultCollection _resultCollection;
        private readonly KafkaMessageConverter _kafkaMessageConverter;
        private readonly Dictionary<Type, IConsumer<string, string>> _cachedConsumers = new();

        public KafkaMessageConsumer(
            ConsumerConfigurationProvider configurationProvider, 
            KafkaMessageConsumerResultCollection resultCollection, 
            KafkaMessageConverter kafkaMessageConverter)
        {
            this._configurationProvider = configurationProvider;
            this._resultCollection = resultCollection;
            this._kafkaMessageConverter = kafkaMessageConverter;
        }

        public Task<TMessage> ConsumeAsync<TMessage>(CancellationToken cancellationToken = default) where TMessage : IMessage, new()
        {
            // We get the configuration outside, so its exception can be thrown outside the task.
            ConsumerConfiguration<TMessage> configuration = this._configurationProvider.GetConfiguration<TMessage>();
            return Task.Run(() =>
            {
                IConsumer<string, string> consumer = this.CreateConsumer(configuration);
                ConsumeResult<string, string> consumeResult = consumer.Consume(cancellationToken);
                
                TMessage message = this._kafkaMessageConverter.ConvertMessage(consumeResult, configuration);
                this._resultCollection.Add(message, consumeResult);
                return message;
            }, cancellationToken);
        }

        public Task CommitAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            if (this._resultCollection.TryGet(message, out ConsumeResult<string, string>? consumeResult))
            {
                ConsumerConfiguration<TMessage> configuration = this._configurationProvider.GetConfiguration<TMessage>();
                IConsumer<string, string> consumer = this.CreateConsumer(configuration);
                consumer.Commit(consumeResult);
            }
            return Task.CompletedTask;
        }

        private  IConsumer<string, string> CreateConsumer<TMessage>(ConsumerConfiguration<TMessage> configuration) where TMessage : IMessage
        {
            if (!this._cachedConsumers.TryGetValue(typeof(TMessage), out IConsumer<string, string>? consumer))
            {
                consumer = new ConsumerBuilder<string, string>(configuration.KafkaConfig).Build();
                consumer.Subscribe(configuration.Topic);

                AssignOffSets(consumer, configuration.TimeStamp, configuration.Timeout);
                this._cachedConsumers.Add(typeof(TMessage), consumer);
            }
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
            foreach (var consumer in this._cachedConsumers.Values)
            {
                consumer.Close();
                consumer.Dispose();
            }
        }
    }
}