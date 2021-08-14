namespace MessageBrokers.Extending
{
    public interface IClientOptions<TSecurityOptions>
        where TSecurityOptions : IClientSecurityOptions
    {
        string? Server { get; set; }

        TSecurityOptions Security { get; init; }
    }
}