using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBrokers.Internals
{
    internal class ScopedBackgroundMessageService<TMessage> : BackgroundService where TMessage : IMessage, new()
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
    internal class BackgroundMessageService<TMessage> where TMessage : IMessage, new() 
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly IMessageProducer _messageProducer;

        public BackgroundMessageService(IMessageConsumer messageConsumer, IMessageProducer messageProducer)
        {
            this._messageConsumer = messageConsumer;
            this._messageProducer = messageProducer;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // This will hang the background service.
                // It will wait for the next available message.
                var message = await this._messageConsumer.ConsumeAsync<TMessage>(stoppingToken);

                await this._messageProducer.PublishAsync(message);
                await this._messageConsumer.CommitAsync(message);
            }
        }
    }
}