using System.Threading.Tasks;

namespace CQRS.Events
{
    public interface IEventDispatcher
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}