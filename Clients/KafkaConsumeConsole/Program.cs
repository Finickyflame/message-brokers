using KafkaConsumeConsole.Events;
using KafkaConsumeConsole.Options;
using KafkaConsumeConsole.Tasks;
using MessageBrokers.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace KafkaConsumeConsole
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
                    .AddApplicationTask<ConsumeOrdersCreatedTask>()
                    .AddOptions<TaskOptions>(builder => builder.BindConfiguration("kafka"))
                    .AddKafkaMessageConsumer(options => options
                        .AddEvent<OrderCreatedEvent>("my-topic", "my-batch-group-id")
                    )
                );

       
    }
}