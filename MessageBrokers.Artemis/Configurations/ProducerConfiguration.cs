using ActiveMQ.Artemis.Client;

namespace MessageBrokers.Artemis.Configurations
{
    internal class ProducerConfiguration<TMessage> where TMessage : IMessage
    {
        public ProducerConfiguration Configuration { get; set; }

        public Message ConvertMessage(TMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}