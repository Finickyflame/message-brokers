using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    internal sealed class ScopedBackgroundMessageService<TMessage> : BackgroundService 
        where TMessage : IMessage, new()
    {
        private readonly IServiceProvider _serviceProvider;

        public ScopedBackgroundMessageService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = this._serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<BackgroundMessageService<TMessage>>();
            await service.ExecuteAsync(stoppingToken);
        }
    }
}