using CQRS.Events.Extending;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Events
{
    public class EventDispatcher : IEventDispatcher
    {
        protected IServiceProvider Services { get; }

        public EventDispatcher(IServiceProvider servicesServices)
        {
            this.Services = servicesServices;
        }

        public virtual async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            await this.PublishToEventHandlers(@event);
        }

        protected async Task PublishToEventHandlers<TEvent>(TEvent @event) where TEvent : IEvent
        {
            IEnumerable<IEventDispatcher<TEvent>> dispatchers = this.Services.GetServices<IEventDispatcher<TEvent>>();
            foreach (IEventDispatcher<TEvent> dispatcher in dispatchers)
            {
                await dispatcher.PublishAsync(@event);
            }
        }
    }
}