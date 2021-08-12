using ActiveMQ.Artemis.Client;
using MessageBrokers.Configurations;
using MessageBrokers.Internals;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers
{
    internal sealed class ArtemisMessageConsumer : IInternalMessageConsumer
    {
        private readonly IConnection _connection;
        private readonly ConsumerConfigurationCollection _configurationCollection;

        public ArtemisMessageConsumer(IConnection  connection, ConsumerConfigurationCollection configurationCollection)
        {
            this._connection = connection;
            this._configurationCollection = configurationCollection;
        }
        
        public async Task<TMessage> ConsumeAsync<TMessage>(CancellationToken cancellationToken = default) where TMessage : IMessage, new()
        {
            ConsumerConfiguration<TMessage> configuration = this._configurationCollection.GetConfiguration<TMessage>();
            IConsumer consumer = await this._connection.CreateConsumerAsync(configuration.Configuration, cancellationToken);
            Message message = await consumer.ReceiveAsync(cancellationToken);
            return configuration.ConvertMessage(message);
        }

        public Task CommitAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            return Task.CompletedTask;
        }
    }
}