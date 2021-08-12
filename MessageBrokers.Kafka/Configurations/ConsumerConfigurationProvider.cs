using Microsoft.Extensions.DependencyInjection;
using System;

namespace MessageBrokers.Kafka.Configurations
{
    internal class ConsumerConfigurationProvider
    {
        private readonly IServiceProvider _services;

        public ConsumerConfigurationProvider(IServiceProvider services)
        {
            this._services = services;
        }

        public ConsumerConfiguration<TMessage> GetConfiguration<TMessage>()
            where TMessage : IMessage
            => this._services.GetRequiredService<ConsumerConfiguration<TMessage>>();
    }
}