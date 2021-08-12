using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Events
{
    internal class InMemoryEventDispatcher : IEventDispatcher
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InMemoryEventDispatcher(IServiceScopeFactory serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public async Task PublishAsync<T>(T @event) where T : IEvent
        {
            using IServiceScope scope = this._serviceScopeFactory.CreateScope();
            IEnumerable<IEventHandler<T>> handlers = scope.ServiceProvider.GetServices<IEventHandler<T>>();
            foreach (IEventHandler<T> handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}