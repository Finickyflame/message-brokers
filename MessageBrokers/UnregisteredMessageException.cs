using System;
using System.Runtime.Serialization;

namespace MessageBrokers
{
    [Serializable]
    public class UnregisteredMessageException<TMessage> : Exception where TMessage : IMessage
    {
        public UnregisteredMessageException()
            : base($"No configuration registered for the specified Message: {typeof(TMessage)}")
        {
        }

        protected UnregisteredMessageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}