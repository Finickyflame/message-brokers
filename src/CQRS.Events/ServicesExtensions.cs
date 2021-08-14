using CQRS.Events.Extending;
using CQRS.Events.Internals;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace CQRS.Events
{
    public static class ServicesExtensions
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

        public static IServiceCollection AddInMemoryEventDispatcher(this IServiceCollection services)
        {
            if (services.Any(service => service.ImplementationType == typeof(IEventDispatcher)))
            {
                return services;
            }
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped(typeof(IEventDispatcher<>), typeof(InMemoryEventDispatcher<>));
            return services;
        }
    }
}