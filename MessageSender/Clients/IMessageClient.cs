using MessageClients.Models;

namespace MessageClients.Clients;

public interface IMessageClient
{
    Task SendMessageAsync(MessageToSendViewModel message);
    Task<List<MessageToGetViewModel>> GetMessagesAsync(DateTime? from, DateTime? to);
}
