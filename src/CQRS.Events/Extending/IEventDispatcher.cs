using System.Threading.Tasks;

namespace CQRS.Events.Extending
{
    public interface IEventDispatcher<in TEvent> where TEvent : IEvent
    {
        Task PublishAsync(TEvent @event);
    }
}