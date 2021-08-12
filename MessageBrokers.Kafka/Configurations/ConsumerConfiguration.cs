using Confluent.Kafka;
using System;
using System.Text.Json;

namespace MessageBrokers.Kafka.Configurations
{
    public record ConsumerConfiguration<TMessage> where TMessage : IMessage
    {
        public ConsumerConfiguration(ClientConfig clientConfig, JsonSerializerOptions serializerOptions, string groupId, string topic)
        {
            this.KafkaConfig = new ConsumerConfig(clientConfig)
            {
                GroupId = groupId,
                EnableAutoCommit = false,
                AutoOffsetReset = AutoOffsetReset.Earliest
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