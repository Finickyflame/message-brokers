﻿using Events;

namespace KafkaConsoleDemo.Events
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

        public long Zipcode { get; set; }
    }
}