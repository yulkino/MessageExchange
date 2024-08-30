using Microsoft.AspNetCore.SignalR.Client;

namespace MessageClients.Clients;

public class MessageHubClient : IMessageHubClient
{
    private readonly HubConnection _hubConnection;

    public MessageHubClient(HubConnection hubConnection)
    {
        _hubConnection = hubConnection;
    }

    public Task ReceiveMessageAsync(Action<string, DateTime, Guid> onMessageReceived)
    {
        _hubConnection.On("ReceiveMessage", onMessageReceived);
        return Task.CompletedTask;
    }
}
