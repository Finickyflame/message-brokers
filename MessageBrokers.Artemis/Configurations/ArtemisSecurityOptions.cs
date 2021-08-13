namespace MessageBrokers.Artemis.Configurations
{
    public record ArtemisSecurityOptions
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}