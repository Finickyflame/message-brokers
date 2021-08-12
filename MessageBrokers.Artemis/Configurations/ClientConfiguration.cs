
namespace MessageBrokers.Artemis.Configurations
{
    public record ClientConfiguration
    {
        public ClientConfiguration()
        {
            this.Security = new SecurityConfiguration();
        }

        public string? Server { get; set; }

        public SecurityConfiguration Security { get; internal set; }
    }
}