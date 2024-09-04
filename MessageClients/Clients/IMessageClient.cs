using MessageClients.Models;

namespace MessageClients.Clients;

public interface IMessageClient
{
    Task<bool> SendMessageAsync(MessageToSendModel message);
    Task<(bool IsSuccessStatusCode, List<MessageToGetModel> FilteredMessages)> GetMessagesAsync(
        DateTime? from = null, DateTime? to = null);
}
