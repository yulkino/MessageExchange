using Microsoft.AspNetCore.SignalR;

namespace MessagesServer.Hubs;

public class MessageHub(IHubContext<MessageHub> hubContext, ILogger<MessageHub> logger) : Hub, IMessageHub
{
    public async Task SendMessageAsync(string message, DateTime timespan, Guid serialNumber)
    {
        logger.LogInformation("Posting {Message} with {Timespan} and {SerialNumber}", message, timespan, serialNumber);

        try
        {
            await hubContext.Clients.All.SendAsync("ReceiveMessage", message, timespan, serialNumber);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Unable to send message");
            throw;
        }

        logger.LogInformation("Message sent successfully");
    }
}
