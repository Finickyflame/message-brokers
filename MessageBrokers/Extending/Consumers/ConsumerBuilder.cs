using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace MessageBrokers.Extending
{
    public abstract class ConsumerBuilder<TConsumerConfiguration, TBuilder> : MessageBrokerBuilder<TBuilder>
        where TConsumerConfiguration : class
        where TBuilder : ConsumerBuilder<TConsumerConfiguration, TBuilder> 
    {
        protected ConsumerBuilder(IServiceCollection services)
            : base(services)
        {
        }

        protected TBuilder AddMessageConsumer<TMessage, TMessageConsumer>()
            where TMessage : class, IMessage, new()
            where TMessageConsumer : class, IMessageConsumer<TMessage>
        {
            this.Services
                .AddScoped<IMessageConsumer<TMessage>, TMessageConsumer>()
                .AddBackgroundMessageService<TMessage>();
            return (TBuilder)this;
        }

        protected TBuilder AddConsumerOptions<TOption>(Func<TConsumerConfiguration, JsonSerializerOptions, TOption> factory, Action<TOption>? configure)
            where TOption : class
        {
            this.Services
                .AddOptions<TOption>()
                .Create<TOption, IOptions<TConsumerConfiguration>, IOptionsMonitor<JsonSerializerOptions>>((configOptions, serializerOptions) =>
                    factory(configOptions.Value, serializerOptions.Get(this.Name))
                )
                .Configure(option => configure?.Invoke(option));
            return (TBuilder)this;
        }

        public TBuilder Configure(Action<TConsumerConfiguration> configure)
        {
            this.Services.Configure(configure);
            return (TBuilder)this;
        }
    }
}