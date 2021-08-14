using CQRS.Events;
using MessageBrokers.Extending;
using MessageBrokers.Internals;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace MessageBrokers
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddMessageHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies.Length == 0)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            services.Scan(source => source
                .FromAssemblies(assemblies)
                .AddClasses(type => type
                    .AssignableTo(typeof(IMessageHandler<>))
                )
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            );
            return services;
        }

        public static IServiceCollection AddMessageHandler<TMessageHandler, TMessage>(this IServiceCollection services)
            where TMessage : IMessage
            where TMessageHandler : class, IMessageHandler<TMessage> => services.AddTransient<IMessageHandler<TMessage>, TMessageHandler>();

        public static IServiceCollection AddInMemoryMessageProducer(this IServiceCollection services)
        {
            if (services.Any(service => service.ImplementationType == typeof(InternalMessageProducer)))
            {
                return services;
            }
            services.AddInMemoryEventDispatcher();
            services.AddScoped<IEventDispatcher, InternalMessageProducer>();
            services.AddScoped<IMessageConsumer, InternalMessageConsumer>();
            services.AddScoped<IMessageProducer, InternalMessageProducer>();
            services.AddScoped(typeof(IMessageProducer<>), typeof(InMemoryMessageProducer<>));
            return services;
        }
    }
}