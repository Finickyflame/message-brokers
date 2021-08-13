using Events;
using MessageBrokers.Extending;
using MessageBrokers.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace MessageBrokers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageHandler<TMessageHandler, TMessage>(this IServiceCollection services)
            where TMessage : IMessage
            where TMessageHandler : class, IMessageHandler<TMessage>
            => services.AddTransient<IMessageHandler<TMessage>, TMessageHandler>();

        internal static IServiceCollection AddBackgroundMessageService<TMessage>(this IServiceCollection services)
            where TMessage : class, IMessage, new()
        {
            services
                .AddHostedService(provider => new ScopedBackgroundMessageService<TMessage>(provider))
                .TryAddScoped<BackgroundMessageService<TMessage>>();
            return services;
        }

        internal static IServiceCollection TryAddMessageBrokerServices(this IServiceCollection services)
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
                .AddScoped<IMessageConsumer, InternalMessageConsumer>();
        }

        internal static IServiceCollection ConfigureBuilder<TBuilder>(this IServiceCollection services, Action<TBuilder> configure)
        where TBuilder : MessageBrokerBuilder<TBuilder>
        {
            var builder = (TBuilder)Activator.CreateInstance(typeof(TBuilder), services)!;
            configure(builder);
            return services;
        }
    }
}