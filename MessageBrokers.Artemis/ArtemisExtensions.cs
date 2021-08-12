using MessageBrokers.Artemis.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBrokers.Artemis
{
    public static class KafkaExtensions
    {
        public static IServiceCollection AddArtemisMessageProducer(this IServiceCollection services/*, Action<ProducerConfigurationBuilder> configure*/)
        {
            /*var builder = new ProducerConfigurationBuilder(services);
            configure(builder);*/
            return services
                .AddCommonServices()
                .AddMessageProducer<ArtemisMessageProducer>();
        }

        public static IServiceCollection AddArtemisMessageConsumer(this IServiceCollection services/*, Action<ConsumerConfigurationBuilder> configure*/)
        {
            /*var builder = new ConsumerConfigurationBuilder(services);
            configure(builder);*/
            return services
                .AddCommonServices()
                .AddMessageConsumer<ArtemisMessageConsumer>();
        }

        private static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services
                .AddOptions<ClientConfiguration>().BindConfiguration("Artemis:client");
            return services;
        }
    }
}