using MessageClients.Models;

namespace MessageClients.Clients;

public class MessageClient : IMessageClient
{
    private readonly HttpClient _httpClient;

    public MessageClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendMessageAsync(MessageToSendViewModel message)
    {
        var response = await _httpClient.PostAsJsonAsync("/message/send", message);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<MessageToGetViewModel>> GetMessagesAsync(DateTime? from, DateTime? to)
    {
        var response = await _httpClient.GetAsync($"/message?from={from:s}&to={to:s}");
        response.EnsureSuccessStatusCode();
        var messages = await response.Content.ReadFromJsonAsync<List<MessageToGetViewModel>>();

        if (messages is null)
            return [];

        return messages;
    }
}
