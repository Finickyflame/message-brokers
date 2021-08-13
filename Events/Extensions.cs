using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

namespace Events
{
    public static class Extensions
    {
        public static IServiceCollection AddEventHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies.Length == 0)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            services.Scan(source => source
                .FromAssemblies(assemblies)
                .AddClasses(type => type
                    .AssignableTo(typeof(IEventHandler<>))
                )
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            );
            return services;
        }

        public static IServiceCollection AddEventHandler<TEventHandler, TEvent>(this IServiceCollection services)
            where TEventHandler : class, IEventHandler<TEvent>
            where TEvent : IEvent
            => services.AddTransient<IEventHandler<TEvent>, TEventHandler>();

        public static IServiceCollection AddInMemoryEventDispatcher(this IServiceCollection services) => services
            .TryAddEventDispatcher();

        internal static IServiceCollection TryAddEventDispatcher(this IServiceCollection services)
        {
            services.TryAddSingleton<InMemoryEventDispatcher>();
            services.TryAddSingleton<IEventDispatcher, InMemoryEventDispatcher>();
            return services;
        }
    }
}