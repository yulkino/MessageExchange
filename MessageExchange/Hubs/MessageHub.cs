using Microsoft.AspNetCore.SignalR;

namespace MessageExchange.Hubs;

public class MessageHub : Hub, IMessageHub
{
    public async Task SendMessageToAll(string message, DateTime timespan, Guid serialNumber)
    {
        await Clients.All.SendAsync("ReceiveMessage", message, timespan, serialNumber);
    }
}
