using MessageClients.Models;

namespace MessageClients.Clients;

public class MessageClient(HttpClient httpClient, ILogger<MessageClient> logger) : IMessageClient
{
    public async Task<bool> SendMessageAsync(MessageToSendModel message)
    {
        logger.LogInformation("Sending message: {Message}", message);
        var response = await httpClient.PostAsJsonAsync("message/send", message);

        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation("Message sent successful");
            return true;
        }

        logger.LogWarning("Unable to send message");
        return false;
    }

    public async Task<(bool IsSuccessStatusCode, List<MessageToGetModel> FilteredMessages)> GetMessagesAsync(DateTime? from = null, DateTime? to = null)
    {
        logger.LogInformation("Getting messages for interval {From}-{To}", from, to);
        var response = await httpClient.GetAsync($"/message?from={from:s}&to={to:s}");

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Unable to get messages");
            return (false, []);
        }

        var messages = await response.Content.ReadFromJsonAsync<List<MessageToGetModel>>() ?? [];

        logger.LogDebug("Got {MessageCount} messages", messages.Count);
        return (true, messages);
    }
}
