using CQRS.Events;

namespace KafkaConsumeConsole.Events
{
    public record OrderCanceledEvent : IEvent
    {
        public long OrderId { get; set; }
    }
}