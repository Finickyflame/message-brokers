using MessageBrokers.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace MessageBrokers.Extending
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddBackgroundMessageService<TMessage>(this IServiceCollection services)
            where TMessage : class, IMessage, new()
        {
            services
                .AddHostedService<ScopedBackgroundMessageService<TMessage>>()
                .TryAddScoped<BackgroundMessageService<TMessage>>();
            return services;
        }

        public static IServiceCollection ConfigureBuilder<TBuilder>(this IServiceCollection services, Action<TBuilder> configure)
            where TBuilder : MessageBrokerBuilder<TBuilder>
        {
            var builder = (TBuilder)Activator.CreateInstance(typeof(TBuilder), services)!;
            configure(builder);
            return services;
        }

        public static IServiceCollection TryAddMessageBrokerClient<TClientOptions, TClientSecurityOptions>(
            this IServiceCollection services,
            string configSectionName,
            Action configureServices
        )
            where TClientOptions : class, IClientOptions<TClientSecurityOptions>
            where TClientSecurityOptions : IClientSecurityOptions
        {
            if (services.Any(service => service.ImplementationType == typeof(IConfigureOptions<TClientOptions>)))
            {
                return services;
            }

            services.AddOptions<TClientOptions>().BindConfiguration(configSectionName + ":client");
            services.AddInMemoryMessageProducer();
            configureServices();
            return services;
        }
    }
}