using Confluent.Kafka;
using MessageBrokers.Extending;
using System;
using System.Text.Json;

namespace MessageBrokers.Kafka.Configurations
{
    // ReSharper disable once UnusedTypeParameter
    public record KafkaConsumerOptions<TMessage> : IConsumerOptions<TMessage> 
        where TMessage : IMessage
    {
        public KafkaConsumerOptions(string topic, string groupId, ConsumerConfig consumerConfig, JsonSerializerOptions serializerOptions)
        {
            this.KafkaConfig = new ConsumerConfig(consumerConfig)
            {
                GroupId = groupId
            };
            this.SerializerOptions = new JsonSerializerOptions(serializerOptions);
            this.Topic = topic;
        }

        public ConsumerConfig KafkaConfig { get; }

        public string Topic { get; }

        public Timestamp? TimeStamp { get; set; }

        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);

        public JsonSerializerOptions SerializerOptions { get; }
    }
}