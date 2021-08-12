using System;
using System.Threading.Tasks;

namespace Events.Internals
{
    internal interface IInternalEventDispatcher : IEventDispatcher
    {
        Task PublishAsync(Type eventType, IEvent @event);
    }
}