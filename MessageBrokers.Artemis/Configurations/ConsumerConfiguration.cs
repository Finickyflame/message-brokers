using ActiveMQ.Artemis.Client;

namespace MessageBrokers.Configurations
{
    internal class ConsumerConfiguration<TMessage> where TMessage : IMessage
    {
        public ConsumerConfiguration Configuration { get; set; }

        public TMessage ConvertMessage(Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}