using MessagesServer.DTOs;

namespace MessagesServer.Services;

public interface IMessageService
{
    Task SendMessageAsync(MessageToSendDto message);
    Task<List<MessageToGetDto>> GetMessagesAsync(DateTime? from, DateTime? to);
}
