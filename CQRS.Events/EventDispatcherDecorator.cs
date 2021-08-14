using System.Threading.Tasks;

namespace CQRS.Events
{
    public abstract class EventDispatcherDecorator : IEventDispatcher
    {
        private readonly IEventDispatcher _base;

        protected EventDispatcherDecorator(IEventDispatcher @base)
        {
            this._base = @base;
        }

        public virtual Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent => this._base.PublishAsync(@event);
    }
}