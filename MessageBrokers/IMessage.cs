using Events;
using System;

namespace MessageBrokers
{
    public interface IMessage
    {
        public string? Key { get; init; }

        public DateTimeOffset? CreatedOn { get; init; }

        public IEvent Value { get; init; }
    }
}