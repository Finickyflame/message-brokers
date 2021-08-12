using Confluent.Kafka;

namespace MessageBrokers.Kafka.Configurations
{
    public record ClientConfiguration
    {
        public ClientConfiguration()
        {
            this.KafkaConfig = new ClientConfig();
            this.Security = new SecurityConfiguration(this.KafkaConfig);
        }

        internal ClientConfig KafkaConfig { get; }

        public string Server
        {
            get => this.KafkaConfig.BootstrapServers;
            set => this.KafkaConfig.BootstrapServers = value;
        }

        public SecurityConfiguration Security { get; internal set; }
    }
}