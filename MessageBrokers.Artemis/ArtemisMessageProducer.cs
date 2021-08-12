using ActiveMQ.Artemis.Client;
using MessageBrokers.Artemis.Configurations;
using System.Threading.Tasks;

namespace MessageBrokers.Artemis
{
    internal sealed class ArtemisMessageProducer : MessageProducer
    {
        private readonly IConnection _connection;
        private readonly ProducerConfigurationCollection _configurationCollection;

        public ArtemisMessageProducer(IMessageProducer @base, IConnection connection, ProducerConfigurationCollection configurationCollection)
            : base(@base)
        {
            this._connection = connection;
            this._configurationCollection = configurationCollection;
        }

        public override async Task PublishAsync<TMessage>(TMessage message)
        {
            if (this._configurationCollection.TryGetConfiguration(out ProducerConfiguration<TMessage> configuration))
            {
                IProducer producer = await this._connection.CreateProducerAsync(configuration.Configuration);
                await producer.SendAsync(configuration.ConvertMessage(message));
            }
            else
            {
                await base.PublishAsync(message);
            }
        }
    }
}