using Confluent.Kafka;
using MessageBrokers.Kafka.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace MessageBrokers.Kafka
{
    public static class KafkaExtensions
    {
        public static IServiceCollection AddKafkaMessageProducer(this IServiceCollection services, Action<KafkaProducerBuilder> configure) => services
            .TryAddKafkaServices()
            .ConfigureBuilder(configure);

        public static IServiceCollection AddKafkaMessageConsumer(this IServiceCollection services, Action<KafkaConsumerBuilder> configure) => services
            .TryAddKafkaServices()
            .ConfigureBuilder(configure);

        private static IServiceCollection TryAddKafkaServices(this IServiceCollection services)
        {
            if (services.Any(service => service.ServiceType == typeof(KafkaMessageConverter<>)))
            {
                return services;
            }

            services.TryAddMessageBrokerServices();
            services.AddSingleton(typeof(KafkaMessageConverter<>));
            services.AddScoped<KafkaMessageCollection>();
            services.AddOptions<KafkaClientOptions>().BindConfiguration("Kafka:client");
            services.AddOptions<ProducerConfig>().Create((IOptions<KafkaClientOptions> options) => new ProducerConfig(options.Value.KafkaConfig));
            services.AddOptions<ConsumerConfig>().Create((IOptions<KafkaClientOptions> options) => new ConsumerConfig(options.Value.KafkaConfig));
            return services;
        }
    }
}