using MessageBrokers.Artemis.Builders;
using MessageBrokers.Artemis.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MessageBrokers.Artemis
{
    public static class KafkaExtensions
    {
        public static IServiceCollection AddArtemisMessageProducer(this IServiceCollection services, Action<ArtemisProducerBuilder> configure) => services
            .TryAddArtemisServices()
            .ConfigureBuilder(configure);

        public static IServiceCollection AddArtemisMessageConsumer(this IServiceCollection services, Action<ArtemisConsumerBuilder> configure) => services
            .TryAddArtemisServices()
            .ConfigureBuilder(configure);

        private static IServiceCollection TryAddArtemisServices(this IServiceCollection services)
        {
            services.TryAddMessageBrokerServices();
            services.AddOptions<ArtemisClientOptions>().BindConfiguration("Artemis:client");
            return services;
        }
    }
}