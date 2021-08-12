using Events;
using MessageBrokers.Internals;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace MessageBrokers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageHandler<TMessageHandler, TMessage>(this IServiceCollection services)
            where TMessage : IMessage
            where TMessageHandler : class, IMessageHandler<TMessage>
            => services.AddTransient<IMessageHandler<TMessage>, TMessageHandler>();


        internal static IServiceCollection AddMessageProducer<TMessageProducer>(this IServiceCollection services)
            where TMessageProducer : MessageProducer
            => services.TryAddRequiredServices().Decorate<IMessageProducer, TMessageProducer>();

        internal static IServiceCollection AddMessageConsumer<TMessageConsumer>(this IServiceCollection services)
            where TMessageConsumer : MessageConsumer
            => services.TryAddRequiredServices().Decorate<IMessageConsumer, TMessageConsumer>();

        internal static IServiceCollection AddBackgroundMessageService<TMessage>(this IServiceCollection services)
            where TMessage : IMessage, new()
            => services.AddHostedService(provider => new BackgroundMessageService<TMessage>(
                provider.GetRequiredService<IMessageConsumer>(),
                provider.GetRequiredService<IMessageProducer>()
            ));

        private static IServiceCollection TryAddRequiredServices(this IServiceCollection services)
        {
            if (services.Any(d => d.ServiceType == typeof(InMemoryMessageProducer)))
            {
                return services;
            }

            return services
                .TryAddEventDispatcher()
                .AddSingleton<InMemoryMessageProducer>()
                .AddSingleton<IMessageProducer, RelayMessageProducer>()
                .AddSingleton<IEventDispatcher, ToMessageEventDispatcher>()
                .AddSingleton<IMessageConsumer, ThrowsUnregisteredMessageConsumer>();
        }
    }
}