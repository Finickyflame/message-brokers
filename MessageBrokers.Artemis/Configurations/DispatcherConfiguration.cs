using ActiveMQ.Artemis.Client;

namespace MessageBrokers.Configurations
{
    internal class DispatcherConfiguration<TMessage> where TMessage : IMessage
    {
        public ProducerConfiguration Configuration { get; set; }

        public Message ConvertMessage(TMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}