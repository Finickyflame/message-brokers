using CQRS.Events.Extending;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Events.Internals
{
    internal class InMemoryEventDispatcher<TEvent> : IEventDispatcher<TEvent> where TEvent : IEvent
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InMemoryEventDispatcher(IServiceScopeFactory serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public async Task PublishAsync(TEvent @event)
        {
            using IServiceScope scope = this._serviceScopeFactory.CreateScope();
            IEnumerable<IEventHandler<TEvent>> handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
            foreach (IEventHandler<TEvent> handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}