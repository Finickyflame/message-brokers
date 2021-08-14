using MessageBrokers.Artemis.Builders;
using MessageBrokers.Artemis.Configurations;
using MessageBrokers.Extending;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MessageBrokers.Artemis
{
    public static class ArtemisExtensions
    {
        public static IServiceCollection AddArtemisMessageProducer(this IServiceCollection services, Action<ArtemisProducerBuilder> configure) => services
            .TryAddArtemisServices()
            .ConfigureBuilder(configure);

        public static IServiceCollection AddArtemisMessageConsumer(this IServiceCollection services, Action<ArtemisConsumerBuilder> configure) => services
            .TryAddArtemisServices()
            .ConfigureBuilder(configure);

        private static IServiceCollection TryAddArtemisServices(this IServiceCollection services)
        {
            return services.TryAddMessageBrokerClient<ArtemisClientOptions, ArtemisClientSecurityOptions>("Artemis", () =>
            {
                services.AddSingleton(typeof(ArtemisMessageConsumerConverter<>));
                services.AddSingleton(typeof(ArtemisMessageProducerConverter<>));
            });
        }
    }
}