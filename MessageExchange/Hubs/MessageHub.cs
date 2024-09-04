using Microsoft.AspNetCore.SignalR;

namespace MessageExchange.Hubs;

public class MessageHub : Hub, IMessageHub
{
    private readonly IHubContext<MessageHub> _hubContext;

    public MessageHub(IHubContext<MessageHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendMessageToAll(string message, DateTime timespan, Guid serialNumber)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message, timespan, serialNumber);
    }
}
