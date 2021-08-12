using KafkaConsoleDemo.Events;
using KafkaConsoleDemo.Options;
using KafkaConsoleDemo.Tasks;
using MessageBrokers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace KafkaConsoleDemo
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
                    .AddTransient<IApplicationTask, ConsumeOrdersCreatedTask>()
                    .AddOptions<TaskOptions>(builder => builder.BindConfiguration("kafka"))
                    .AddKafkaMessageConsumer(options => options
                        .AddEvent<OrderCreatedEvent>("my-group-id", "my-topic")
                    )
                );

       
    }
}