﻿using Events;
using KafkaConsumeWorker.Events;
using MessageBrokers.Kafka;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace KafkaConsumeWorker
{
    public static class Program
    {
        public static Task Main(string[] args) =>
            CreateHostBuilder(args)
                .Build()
                .RunWithTasksAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddEventHandler<OrderCreatedEvent, OrderCreatedEventHandler>()
                    .AddKafkaMessageConsumer(options => options
                        .AddEvent<OrderCreatedEvent>("my-topic", "my-worker-group-id")
                    )
                );

       
    }
}