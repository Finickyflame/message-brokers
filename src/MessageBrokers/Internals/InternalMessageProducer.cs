using CQRS.Events.Internals;
using MessageBrokers.Extending;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    internal sealed class InternalMessageProducer : InternalEventDispatcher, IMessageProducer
    {
        public InternalMessageProducer(IServiceProvider services)
            : base(services)
        {
        }

        async Task IMessageProducer.PublishAsync<TMessage>(TMessage message)
        {
            await this.PublishToMessageHandlers(message);
            if (message.Value is not null)
            {
                await this.PublishToEventHandlers((dynamic)message.Value);
            }
        }

        public override async Task PublishAsync<TEvent>(TEvent @event)
        {
            await this.PublishToMessageHandlers(new Message<TEvent>(@event));
            await this.PublishToEventHandlers(@event);
        }

        private async Task PublishToMessageHandlers<TMessage>(TMessage message) where TMessage : IMessage
        {
            IEnumerable<IMessageProducer<TMessage>> producers = this.Services.GetServices<IMessageProducer<TMessage>>();
            foreach (IMessageProducer<TMessage> producer in producers)
            {
                await producer.PublishAsync(message);
            }
        }
    }
}