namespace MessageBrokers.Extending
{
    public interface IClientOptions<TSecurityOptions>
        where TSecurityOptions : IClientSecurityOptions
    {
        string? Server { get; set; }

        TSecurityOptions Security { get; init; }
    }

    public interface IClientSecurityOptions
    {
        string? Username { get; set; }

        string? Password { get; set; }
    }
}