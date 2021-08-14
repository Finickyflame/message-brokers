using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace MessageBrokers.Extending
{
    public abstract class ProducerBuilder<TProducerConfiguration, TBuilder> : MessageBrokerBuilder<TBuilder>
        where TProducerConfiguration : class
        where TBuilder : ProducerBuilder<TProducerConfiguration, TBuilder>
    {
        protected ProducerBuilder(IServiceCollection services)
            : base(services)
        {
        }

        protected TBuilder AddMessageProducer<TMessage, TMessageProducer>()
            where TMessage : class, IMessage
            where TMessageProducer : class, IMessageProducer<TMessage>
        {
            this.Services
                .AddScoped<IMessageProducer<TMessage>, TMessageProducer>();
            return (TBuilder)this;
        }

        protected TBuilder AddProducerOptions<TMessage, TProducerOptions>(Func<TProducerConfiguration, JsonSerializerOptions, TProducerOptions> factory, Action<TProducerOptions>? configure)
            where TMessage : class, IMessage
            where TProducerOptions : class, IProducerOptions<TMessage>
        {
            this.Services
                .AddOptions<TProducerOptions>()
                .Create<TProducerOptions, IOptions<TProducerConfiguration>, IOptionsMonitor<JsonSerializerOptions>>((configOptions, serializerOptions) =>
                    factory(configOptions.Value, serializerOptions.Get(this.Name))
                )
                .Configure(option => configure?.Invoke(option));
            return (TBuilder)this;
        }

        public TBuilder Configure(Action<TProducerConfiguration> configure)
        {
            this.Services.Configure(configure);
            return (TBuilder)this;
        }
    }
}