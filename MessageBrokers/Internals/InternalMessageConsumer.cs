using MessageBrokers.Extending;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    internal sealed class InternalMessageConsumer : IMessageConsumer
    {
        private readonly IServiceProvider _services;

        public InternalMessageConsumer(IServiceProvider services)
        {
            this._services = services;
        }

        public async Task<TMessage> ConsumeAsync<TMessage>(CancellationToken cancellationToken = default) where TMessage : IMessage, new()
        {
            // We can only consume from one source, so we take the last registered source.
            var consumer = this._services.GetService<IMessageConsumer<TMessage>>();
            if (consumer is null)
            {
                throw new UnregisteredMessageException<TMessage>();
            }
            return await consumer.ConsumeAsync(cancellationToken);
        }

        public async Task CommitAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            var consumer = this._services.GetService<IMessageConsumer<TMessage>>();
            if (consumer is null)
            {
                throw new UnregisteredMessageException<TMessage>();
            }
            await consumer.CommitAsync(message);
        }
    }
}