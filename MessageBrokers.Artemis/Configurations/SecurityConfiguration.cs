namespace MessageBrokers.Artemis.Configurations
{
    public record SecurityConfiguration
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}