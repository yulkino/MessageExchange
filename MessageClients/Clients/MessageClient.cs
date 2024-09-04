using MessageClients.Models;

namespace MessageClients.Clients;

public class MessageClient : IMessageClient
{
    private readonly HttpClient _httpClient;

    public MessageClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> SendMessageAsync(MessageToSendModel message)
    {
        var response = await _httpClient.PostAsJsonAsync("message/send", message);

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    public async Task<(bool IsSuccessStatusCode, List<MessageToGetModel> FilteredMessages)> GetMessagesAsync(DateTime? from = null, DateTime? to = null)
    {
        var response = await _httpClient.GetAsync($"/message?from={from:s}&to={to:s}");

        if (!response.IsSuccessStatusCode)
            return (false, []);

        var messages = await response.Content.ReadFromJsonAsync<List<MessageToGetModel>>();

        return (true, messages ?? []);
    }
}
