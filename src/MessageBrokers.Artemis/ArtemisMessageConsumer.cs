using ActiveMQ.Artemis.Client;
using MessageBrokers.Artemis.Configurations;
using MessageBrokers.Extending;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers.Artemis
{
    internal sealed class ArtemisMessageConsumer<TMessage> : IMessageConsumer<TMessage>
        where TMessage : IMessage, new()
    {
        private readonly ArtemisConsumerOptions<TMessage> _options;
        private readonly IConnection _connection;
        private readonly ArtemisMessageConsumerConverter<TMessage> _converter;

        public ArtemisMessageConsumer(ArtemisConsumerOptions<TMessage> options, IConnection connection, ArtemisMessageConsumerConverter<TMessage> converter)
        {
            this._options = options;
            this._connection = connection;
            this._converter = converter;
        }

        public async Task<TMessage> ConsumeAsync(CancellationToken cancellationToken = default)
        {
            IConsumer consumer = await this._connection.CreateConsumerAsync(this._options.Configuration, cancellationToken);
            Message message = await consumer.ReceiveAsync(cancellationToken);
            return this._converter.ConvertMessage(message);
        }

        public Task CommitAsync(TMessage message)
        {
            return Task.CompletedTask;
        }
    }
}