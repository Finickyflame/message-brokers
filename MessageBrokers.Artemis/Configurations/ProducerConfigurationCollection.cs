namespace MessageBrokers.Artemis.Configurations
{
    internal class ProducerConfigurationCollection
    {
        public bool TryGetConfiguration<TMessage>(out ProducerConfiguration<TMessage> configuration) where TMessage : IMessage
        {
            throw new System.NotImplementedException();
        }
    }
}