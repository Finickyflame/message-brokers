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
        private readonly ArtemisMessageConverter<TMessage> _converter;
        private readonly Lazy<Task<IProducer>> _producer;

        public ArtemisMessageProducer(IConnection connection, IOptions<ArtemisProducerOptions<TMessage>> options, ArtemisMessageConverter<TMessage> converter)
        {
            this._converter = converter;
            this._producer = new Lazy<Task<IProducer>>(async () => await connection.CreateProducerAsync(options.Value.Configuration));
        }

        public async Task PublishAsync(TMessage message)
        {
            IProducer producer = await this._producer.Value;
            await producer.SendAsync(this._converter.ConvertMessage(message));
        }

        public async ValueTask DisposeAsync()
        {
            if (this._producer.IsValueCreated)
            {
                await (await this._producer.Value).DisposeAsync();
            }
        }
    }
}