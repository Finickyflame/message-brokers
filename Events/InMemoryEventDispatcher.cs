using Events.Internals;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Events
{
    internal class InMemoryEventDispatcher : IInternalEventDispatcher
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

        public async Task PublishAsync(Type eventType, IEvent @event)
        {
            using IServiceScope scope = this._serviceScopeFactory.CreateScope();
            IEnumerable<dynamic> handlers = scope.ServiceProvider.GetServices(HandlerType(eventType));
            foreach (dynamic handler in handlers)
            {
                await handler!.HandleAsync((dynamic)@event);
            }
        }

        private static Type HandlerType(Type eventType) => typeof(IEventHandler<>).MakeGenericType(eventType);
    }
}