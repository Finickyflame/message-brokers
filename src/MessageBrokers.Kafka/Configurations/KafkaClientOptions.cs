using Confluent.Kafka;
using MessageBrokers.Extending;

namespace MessageBrokers.Kafka.Configurations
{
    public record KafkaClientOptions : IClientOptions<KafkaClientSecurityOptions>
    {
        public KafkaClientOptions()
        {
            this.KafkaConfig = new ClientConfig();
            this.Security = new KafkaClientSecurityOptions(this.KafkaConfig);
        }

        internal ClientConfig KafkaConfig { get; }

        public string? Server
        {
            get => this.KafkaConfig.BootstrapServers;
            set => this.KafkaConfig.BootstrapServers = value;
        }

        public KafkaClientSecurityOptions Security { get; init; }
    }
}