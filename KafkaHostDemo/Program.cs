using Events;
using KafkaHostDemo.Events;
using MessageBrokers;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace KafkaHostDemo
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
                    .AddEventHandler<OrderCreatedEvent, ImpairedChangedEventHandler>()
                    .AddKafkaMessageConsumer(options => options
                        .AddEvent<OrderCreatedEvent>("my-group-id", "my-topic")
                    )
                );

       
    }
}