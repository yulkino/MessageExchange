using Microsoft.AspNetCore.SignalR;

namespace MessageExchange.Hubs;

public class MessageHub : Hub, IMessageHub
{
    private readonly IHubContext<MessageHub> _hubContext;
    private readonly ILogger<MessageHub> _logger;

    public MessageHub(IHubContext<MessageHub> hubContext, ILogger<MessageHub> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SendMessageToAll(string message, DateTime timespan, Guid serialNumber)
    {
        _logger.LogInformation("Posting {Message} with {Timespan} and {SerialNumber}", message, timespan, serialNumber);

        try
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message, timespan, serialNumber);
        }
        catch(Exception ex)
        {
            _logger.LogError("During posting message error occurred: {Error}", ex);
            throw;
        }

        _logger.LogInformation("Message posted successfully");
    }
}
