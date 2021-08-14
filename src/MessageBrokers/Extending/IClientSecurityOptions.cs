namespace MessageBrokers.Extending
{
    public interface IClientSecurityOptions
    {
        string? Username { get; set; }

        string? Password { get; set; }
    }
}