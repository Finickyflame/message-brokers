using Confluent.Kafka;

namespace MessageBrokers.Kafka.Configurations
{
    public record KafkaClientOptions
    {
        public KafkaClientOptions()
        {
            this.KafkaConfig = new ClientConfig();
            this.Security = new KafkaSecurityOptions(this.KafkaConfig);
        }

        internal ClientConfig KafkaConfig { get; }

        public string Server
        {
            get => this.KafkaConfig.BootstrapServers;
            set => this.KafkaConfig.BootstrapServers = value;
        }

        public KafkaSecurityOptions Security { get; internal set; }
    }
}