using Events.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Events
{
    public static class Extensions
    {
        public static IServiceCollection AddEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
            where TEvent : IEvent
            where TEventHandler : class, IEventHandler<TEvent>
            => services.AddTransient<IEventHandler<TEvent>, TEventHandler>();

        public static IServiceCollection AddInMemoryEventDispatcher(this IServiceCollection services) => services
                .AddSingleton<IEventDispatcher, InMemoryEventDispatcher>()
                .AddInternalEventDispatcher();

        internal static IServiceCollection AddInternalEventDispatcher(this IServiceCollection services)
        {
            services.TryAddSingleton<IInternalEventDispatcher, InMemoryEventDispatcher>();
            return services;
        }
    }
}