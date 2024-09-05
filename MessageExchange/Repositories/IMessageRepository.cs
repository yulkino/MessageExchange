using MessageExchange.DAOs;

namespace MessageExchange.Repositories;

public interface IMessageRepository
{
    Task AddMessageAsync(MessageDao message);
    Task<List<MessageDao>> GetMessagesForPeriodAsync(DateTime from, DateTime to);
    Task<List<MessageDao>> GetMessagesAfterAsync(DateTime from);
    Task<List<MessageDao>> GetMessagesBeforeAsync(DateTime to);
    Task<List<MessageDao>> GetAllMessagesAsync();
}
