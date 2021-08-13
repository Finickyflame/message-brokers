using Bogus;
using KafkaProduceConsole.Events;
using KafkaProduceConsole.Options;
using KafkaProduceConsole.Tasks;
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
                    .AddApplicationTask<ProduceOrdersCreatedTask>()
                    .AddSingleton(_ => new Faker<Address>()
                        .RuleFor(address => address.City, faker => faker.Address.City())
                        .RuleFor(address => address.State, faker => faker.Address.State())
                        .RuleFor(address => address.Zipcode, faker => faker.Address.ZipCode())
                    )
                    .AddSingleton(service => new Faker<OrderCreatedEvent>()
                        .RuleFor(order => order.Ordertime, faker => faker.Date.Recent().Ticks)
                        .RuleFor(order => order.Orderid, faker => faker.Commerce.Random.Long())
                        .RuleFor(order => order.Itemid, faker => faker.Commerce.Ean8())
                        .RuleFor(order => order.Address, () => service.GetRequiredService<Faker<Address>>())
                    )
                    .AddOptions<TaskOptions>(builder => builder.BindConfiguration(string.Empty))
                    .AddKafkaMessageProducer(options => options
                        .AddEvent<OrderCreatedEvent>("my-topic")
                    )
                );
    }
}