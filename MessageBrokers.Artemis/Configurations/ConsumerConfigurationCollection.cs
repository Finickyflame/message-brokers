using System.Diagnostics.CodeAnalysis;

namespace MessageBrokers.Artemis.Configurations
{
    internal class ConsumerConfigurationCollection
    {
        public bool TryGetConfiguration<TMessage>([NotNullWhen(true)]out ConsumerConfiguration<TMessage>? configuration) where TMessage : IMessage
        {
            throw new System.NotImplementedException();
        }
    }
}