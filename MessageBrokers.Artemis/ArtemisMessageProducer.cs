using ActiveMQ.Artemis.Client;
using MessageBrokers.Configurations;
using System.Threading.Tasks;

namespace MessageBrokers
{
    internal sealed class ArtemisMessageProducer : IMessageProducer
    {
        private readonly IConnection _connection;
        private readonly DispatcherConfigurationCollection _configurationCollection;

        public ArtemisMessageProducer(IConnection connection, DispatcherConfigurationCollection configurationCollection)
        {
            this._connection = connection;
            this._configurationCollection = configurationCollection;
        }

        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            if (this._configurationCollection.TryGetConfiguration(out DispatcherConfiguration<TMessage> configuration))
            {
                IProducer producer = await this._connection.CreateProducerAsync(configuration.Configuration);
                await producer.SendAsync(configuration.ConvertMessage(message));
            }
        }
    }
}