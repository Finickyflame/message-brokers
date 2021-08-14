using MessageBrokers.Extending;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    internal sealed class InMemoryMessageProducer<TMessage> : IMessageProducer<TMessage>
        where TMessage : IMessage
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InMemoryMessageProducer(IServiceScopeFactory serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public async Task PublishAsync(TMessage message)
        {
            using IServiceScope scope = this._serviceScopeFactory.CreateScope();
            IEnumerable<IMessageHandler<TMessage>> handlers = scope.ServiceProvider.GetServices<IMessageHandler<TMessage>>();
            foreach (IMessageHandler<TMessage> handler in handlers)
            {
                await handler.HandleAsync(message);
            }
        }
    }
}