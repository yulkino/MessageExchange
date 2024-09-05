using MessageExchange.DTOs;

namespace MessageExchange.Services;

public interface IMessageService
{
    Task SendMessage(MessageToSendDto message);
    Task<List<MessageToGetDto>> GetMessages(DateTime? from, DateTime? to);
}
