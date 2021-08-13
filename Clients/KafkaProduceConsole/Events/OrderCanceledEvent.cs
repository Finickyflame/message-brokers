using Events;

namespace KafkaProduceConsole.Events
{
    public record OrderCanceledEvent : IEvent
    {
        public long OrderId { get; set; }
    }
}