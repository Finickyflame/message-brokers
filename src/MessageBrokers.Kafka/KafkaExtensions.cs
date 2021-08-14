using Confluent.Kafka;
using MessageBrokers.Extending;
using MessageBrokers.Kafka.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

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
            return services.TryAddMessageBrokerClient<KafkaClientOptions, KafkaClientSecurityOptions>("Kafka", () =>
            {
                services.AddScoped<KafkaMessageCollection>();
                services.AddSingleton(typeof(KafkaMessageConsumerConverter<>));
                services.AddSingleton(typeof(KafkaMessageProducerConverter<>));
                services.AddOptions<ProducerConfig>().Create((IOptions<KafkaClientOptions> options) => new ProducerConfig(options.Value.KafkaConfig));
                services.AddOptions<ConsumerConfig>().Create((IOptions<KafkaClientOptions> options) => new ConsumerConfig(options.Value.KafkaConfig));
            });
        }
    }
}