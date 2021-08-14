using MessageBrokers.Extending;

namespace MessageBrokers.Artemis.Configurations
{
    public record ArtemisClientSecurityOptions : IClientSecurityOptions
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}