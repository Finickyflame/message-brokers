using MessageBrokers.Extending;

namespace MessageBrokers.Artemis.Configurations
{
    public record ArtemisClientOptions : IClientOptions<ArtemisClientSecurityOptions>
    {
        public ArtemisClientOptions()
        {
            this.Security = new ArtemisClientSecurityOptions();
        }

        public string? Server { get; set; }

        public ArtemisClientSecurityOptions Security { get; init; }
    }
}