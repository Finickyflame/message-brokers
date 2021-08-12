using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MessageBrokers.Kafka.Configurations
{
    internal class ProducerConfigurationProvider
    {
        private readonly IServiceProvider _services;

        public ProducerConfigurationProvider(IServiceProvider services)
        {
            this._services = services;
        }
        
        public bool TryGetConfiguration<TMessage>([NotNullWhen(true)]out ProducerConfiguration<TMessage>? configuration) where TMessage : IMessage
        {
            configuration = this._services.GetService<ProducerConfiguration<TMessage>>();
            return configuration != null;
        }
    }
}