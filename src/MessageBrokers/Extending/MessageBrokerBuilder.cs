using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace MessageBrokers.Extending
{
    public abstract class MessageBrokerBuilder<TBuilder> 
        where TBuilder : MessageBrokerBuilder<TBuilder> 
    {
        protected MessageBrokerBuilder(IServiceCollection services)
        {
            this.Services = services;
            this.Name = this.GetType().Name;
            this.ConfigureSerializer(options => options.PropertyNameCaseInsensitive = true);
        }

        protected string Name { get; }

        public IServiceCollection Services { get; }

        public TBuilder ConfigureSerializer(Action<JsonSerializerOptions> configure)
        {
            this.Services.Configure(this.Name, configure);
            return (TBuilder)this;
        }
    }
}