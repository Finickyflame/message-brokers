using Events;

namespace KafkaConsumeConsole.Events
{
    public record OrderCreatedEvent : IEvent
    {
        public long Ordertime { get; set; }

        public long Orderid { get; set; }

        public string Itemid { get; set; }

        public Address Address { get; set; }
    }

    public record Address
    {
        public string City { get; set; }

        public string State { get; set; }

        public string Zipcode { get; set; }
    }
}