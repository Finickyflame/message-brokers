using Confluent.Kafka;
using MessageBrokers.Kafka.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace MessageBrokers.Kafka
{
    public static class KafkaExtensions
    {
        public static IServiceCollection AddKafkaMessageProducer(this IServiceCollection services, Action<ProducerConfigurationBuilder> configure)
        {
            var builder = new ProducerConfigurationBuilder(services);
            configure(builder);
            services.TryAddSingleton<ProducerConfigurationProvider>();
            return services
                .AddCommonServices()
                .AddMessageProducer<KafkaMessageProducer>()
                .AddSingleton<ProducerConfigurationProvider>()
                .AddSingleton(serviceProvider =>
                {
                    ClientConfiguration clientConfiguration = serviceProvider.GetRequiredService<IOptions<ClientConfiguration>>().Value;
                    return new ProducerConfig(clientConfiguration.KafkaConfig);
                });
        }

        public static IServiceCollection AddKafkaMessageConsumer(this IServiceCollection services, Action<ConsumerConfigurationBuilder> configure)
        {
            var builder = new ConsumerConfigurationBuilder(services);
            configure(builder);
            return services
                .AddCommonServices()
                .AddMessageConsumer<KafkaMessageConsumer>()
                .AddTransient<KafkaMessageConsumerResultCollection>()
                .AddSingleton<ConsumerConfigurationProvider>()
                .AddSingleton(serviceProvider =>
                {
                    ClientConfiguration clientConfiguration = serviceProvider.GetRequiredService<IOptions<ClientConfiguration>>().Value;
                    return new ConsumerConfig(clientConfiguration.KafkaConfig);
                });
        }

        private static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services
                .AddTransient<KafkaMessageConverter>()
                .AddOptions<ClientConfiguration>().BindConfiguration("Kafka:client");
            return services;
        }
    }
}