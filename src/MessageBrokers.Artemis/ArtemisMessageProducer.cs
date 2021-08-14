using ActiveMQ.Artemis.Client;
using MessageBrokers.Artemis.Configurations;
using MessageBrokers.Extending;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace MessageBrokers.Artemis
{
    internal sealed class ArtemisMessageProducer<TMessage> : IMessageProducer<TMessage>, IAsyncDisposable
        where TMessage : IMessage, new()
    {
        private readonly IConnection _connection;
        private readonly ArtemisMessageProducerConverter<TMessage> _converter;
        private IProducer? _producer;
        private readonly ArtemisProducerOptions<TMessage> _options;

        public ArtemisMessageProducer(IConnection connection, IOptions<ArtemisProducerOptions<TMessage>> options, ArtemisMessageProducerConverter<TMessage> converter)
        {
            this._options = options.Value;
            this._connection = connection;
            this._converter = converter;
        }

        public async Task PublishAsync(TMessage message)
        {
            this._producer ??= await this._connection.CreateProducerAsync(this._options.Configuration);
            await this._producer.SendAsync(this._converter.ConvertMessage(message));
        }

        public async ValueTask DisposeAsync()
        {
            if (this._producer is not null)
            {
                await this._producer.DisposeAsync();
            }
        }
    }
}