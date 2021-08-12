namespace MessageBrokers.Configurations
{
    internal class ConsumerConfigurationCollection
    {
        public ConsumerConfiguration<TMessage> GetConfiguration<TMessage>() where TMessage : IMessage
        {
            throw new System.NotImplementedException();
        }
    }
}