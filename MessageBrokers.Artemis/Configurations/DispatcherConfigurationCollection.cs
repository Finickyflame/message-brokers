namespace MessageBrokers.Configurations
{
    internal class DispatcherConfigurationCollection
    {
        public bool TryGetConfiguration<TMessage>(out DispatcherConfiguration<TMessage> configuration) where TMessage : IMessage
        {
            throw new System.NotImplementedException();
        }
    }
}