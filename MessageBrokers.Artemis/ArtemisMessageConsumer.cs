using ActiveMQ.Artemis.Client;
using MessageBrokers.Artemis.Configurations;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers.Artemis
{
    internal sealed class ArtemisMessageConsumer : MessageConsumer
    {
        private readonly IConnection _connection;
        private readonly ConsumerConfigurationCollection _configurationCollection;

        public ArtemisMessageConsumer(IMessageConsumer @base, IConnection  connection, ConsumerConfigurationCollection configurationCollection)
            :base(@base)
        {
            this._connection = connection;
            this._configurationCollection = configurationCollection;
        }
        
        public override async Task<TMessage> ConsumeAsync<TMessage>(CancellationToken cancellationToken = default)
        {
            if (!this._configurationCollection.TryGetConfiguration(out ConsumerConfiguration<TMessage>? configuration))
            {
                return await base.ConsumeAsync<TMessage>(cancellationToken);
            }

            IConsumer consumer = await this._connection.CreateConsumerAsync(configuration.Configuration, cancellationToken);
            Message message = await consumer.ReceiveAsync(cancellationToken);
            return configuration.ConvertMessage(message);
        }

        public override async Task CommitAsync<TMessage>(TMessage message)
        {
            if (!this._configurationCollection.TryGetConfiguration(out ConsumerConfiguration<TMessage>? _))
            {
                await base.CommitAsync(message);
            }
        }
    }
}