namespace MessageClients.Clients;

public interface IMessageHubClient
{
    Task ReceiveMessageAsync(Action<string, DateTime, Guid> onMessageReceived);
}
