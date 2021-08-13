using Events;

namespace KafkaConsumeWorker.Events
{
    public record OrderCanceledEvent : IEvent
    {
        public long OrderId { get; set; }
    }
}