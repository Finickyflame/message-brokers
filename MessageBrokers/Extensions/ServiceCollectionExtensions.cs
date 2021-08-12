using Events;
using MessageBrokers.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBrokers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageHandler<TMessageHandler, TMessage>(this IServiceCollection services)
            where TMessage : IMessage
            where TMessageHandler : class, IMessageHandler<TMessage>
            => services.AddTransient<IMessageHandler<TMessage>, TMessageHandler>();

        internal static IServiceCollection AddMessageProducer<TMessageProducer>(this IServiceCollection services)
            where TMessageProducer : class, IInternalMessageProducer
            => services
                .AddInternalEventDispatcher()
                .AddTransient<IMessageProducer, RelayMessageProducer>()
                .AddTransient<IEventDispatcher, RelayEventDispatcher>()
                .AddTransient<IInternalMessageProducer, TMessageProducer>();

        internal static IServiceCollection AddMessageConsumer<TMessageConsumer>(this IServiceCollection services)
            where TMessageConsumer : class, IInternalMessageConsumer
        {
            return services
                .AddInternalEventDispatcher()
                .AddMessageProducer<InMemoryMessageProducer>()
                .AddTransient<IMessageConsumer, TMessageConsumer>()
                .AddTransient<IInternalMessageConsumer, TMessageConsumer>();
        }

        internal static IServiceCollection AddBackgroundMessageService<TMessage>(this IServiceCollection services)
            where TMessage : IMessage, new()
            => services.AddHostedService(provider => new BackgroundMessageService<TMessage>(
                provider.GetRequiredService<IInternalMessageConsumer>(),
                provider.GetRequiredService<IMessageProducer>()
            ));
    }
}