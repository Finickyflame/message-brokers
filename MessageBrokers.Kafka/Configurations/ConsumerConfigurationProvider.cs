using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MessageBrokers.Kafka.Configurations
{
    internal class ConsumerConfigurationProvider
    {
        private readonly IServiceProvider _services;

        public ConsumerConfigurationProvider(IServiceProvider services)
        {
            this._services = services;
        }

        public bool TryGetConfiguration<TMessage>([NotNullWhen(true)] out ConsumerConfiguration<TMessage>? configuration)
            where TMessage : IMessage
        {
            configuration = this._services.GetService<ConsumerConfiguration<TMessage>>();
            return configuration != null;
        }
    }
}