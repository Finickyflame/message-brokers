namespace MessageBrokers.Extending
{
    public interface IMessageConsumerConverter<in TSource, out TMessage>
        where TMessage : IMessage, new()
    {
        TMessage ConvertMessage(TSource message);
    }
    
    public interface IMessageProducerConverter<out TTarget, in TMessage>
        where TMessage : IMessage
    {
        TTarget ConvertMessage(TMessage message);
    }
}