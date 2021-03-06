using Bogus;
using KafkaProduceConsole.Events;
using KafkaProduceConsole.Options;
using MessageBrokers.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace KafkaProduceConsole
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
                    .AddFakers()
                    .AddApplicationTasks()
                    .AddOptions<TaskOptions>(builder => builder.BindConfiguration(string.Empty))
                    .AddKafkaMessageProducer(options => options
                        .AddEvent<OrderCreatedEvent>(topic: "topic-order-created")
                        .AddEvent<OrderCanceledEvent>(topic: "topic-order-canceled")
                    )
                );

        private static IServiceCollection AddFakers(this IServiceCollection services) => services
            .AddSingleton(_ => new Faker<Address>()
                .RuleFor(address => address.City, faker => faker.Address.City())
                .RuleFor(address => address.State, faker => faker.Address.State())
                .RuleFor(address => address.Zipcode, faker => faker.Address.ZipCode())
            )
            .AddSingleton(service => new Faker<OrderCreatedEvent>()
                .RuleFor(order => order.OrderTime, faker => faker.Date.Recent())
                .RuleFor(order => order.OrderId, faker => faker.Commerce.Random.Long())
                .RuleFor(order => order.ItemId, faker => faker.Commerce.Ean8())
                .RuleFor(order => order.Address, () => service.GetRequiredService<Faker<Address>>())
            )
            .AddSingleton(_ => new Faker<OrderCanceledEvent>()
                .RuleFor(order => order.OrderId, faker => faker.Commerce.Random.Long())
            );
    }
}