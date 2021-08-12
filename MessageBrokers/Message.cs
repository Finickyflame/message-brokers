using Events;
using System;

namespace MessageBrokers
{
    public record Message<TEvent> : IMessage where TEvent : IEvent
    {
        public Message()
        {
            this.Key = string.Empty;
        }

        public Message(TEvent value)
        {
            this.Key = string.Empty;
            this.Value = value;
        }

        public Message(string key, TEvent value)
        {
            this.Key = key;
            this.Value = value;
        }

        public string Key { get; init; }
        
        public DateTimeOffset CreatedOn { get; init; }

        public TEvent Value { get; init; }

        IEvent IMessage.Value
        {
            get => this.Value;
            init => this.Value = (TEvent)value;
        }
    }
}