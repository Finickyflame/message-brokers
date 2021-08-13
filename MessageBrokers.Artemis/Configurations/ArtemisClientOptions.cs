
namespace MessageBrokers.Artemis.Configurations
{
    public record ArtemisClientOptions
    {
        public ArtemisClientOptions()
        {
            this.Security = new ArtemisSecurityOptions();
        }

        public string? Server { get; set; }

        public ArtemisSecurityOptions Security { get; internal set; }
    }
}