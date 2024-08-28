using MessageSender.Models;

namespace MessageSender.Clients;

public interface IMessageClient
{
    Task SendMessageAsync(MessageToSendViewModel message);
    Task<List<MessageToGetViewModel>> GetMessagesAsync(DateTime? from, DateTime? to);
}
