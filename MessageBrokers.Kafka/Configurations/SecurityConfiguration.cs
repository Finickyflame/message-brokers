using Confluent.Kafka;

namespace MessageBrokers.Kafka.Configurations
{
    public record SecurityConfiguration
    {
        public SecurityConfiguration(ClientConfig kafkaConfig)
        {
            this.KafkaConfig = kafkaConfig;
        }

        private ClientConfig KafkaConfig { get; }

        public SecurityProtocol? Protocol 
        { 
            get => this.KafkaConfig.SecurityProtocol;
            set => this.KafkaConfig.SecurityProtocol = value;
        }
        
        public SaslMechanism? Mechanism 
        { 
            get => this.KafkaConfig.SaslMechanism;
            set => this.KafkaConfig.SaslMechanism = value;
        }
        
        public string? Username 
        { 
            get => this.KafkaConfig.SaslUsername;
            set => this.KafkaConfig.SaslUsername = value;
        }
        
        public string? Password 
        { 
            get => this.KafkaConfig.SaslPassword;
            set => this.KafkaConfig.SaslPassword = value;
        }
    }
}