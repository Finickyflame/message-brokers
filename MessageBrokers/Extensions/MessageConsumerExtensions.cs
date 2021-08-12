using Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers
{
    public static class MessageConsumerExtensions
    {
        public static async IAsyncEnumerable<TMessage> ConsumeAllAsync<TMessage>(this IMessageConsumer messageConsumer, TimeSpan maxDelayPerConsume) where TMessage : IMessage, new()
        {
            
                CancellationTokenSource ctx;
                do
                {
                    ctx = new CancellationTokenSource(maxDelayPerConsume);
                    TMessage message;
                    try
                    {
                        message = await messageConsumer.ConsumeAsync<TMessage>(ctx.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        yield break;
                    }
                    yield return message;
                } while (!ctx.IsCancellationRequested);
        }
        
        public static IAsyncEnumerable<TEvent> ConsumeAllEventsAsync<TEvent>(this IMessageConsumer messageConsumer, TimeSpan maxDelayPerConsume) where TEvent : IEvent => 
            messageConsumer.ConsumeAllAsync<Message<TEvent>>(maxDelayPerConsume).Select(message => message.Value);

        public static async Task<TEvent> ConsumeEventAsync<TEvent>(this IMessageConsumer messageConsumer, CancellationToken cancellationToken) where TEvent : IEvent => 
            (await messageConsumer.ConsumeAsync<Message<TEvent>>(cancellationToken)).Value;
    }
}