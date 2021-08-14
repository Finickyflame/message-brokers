using CQRS.Events;
using System;

namespace KafkaConsumeConsole.Events
{
    public record OrderCreatedEvent : IEvent
    {
        public DateTimeOffset OrderTime { get; set; }

        public long OrderId { get; set; }

        public string? ItemId { get; set; }

        public Address Address { get; set; } = new();
    }

    public record Address
    {
        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }
    }
}