using MessagesServer.Mapping;
using MessagesServer.DTOs;
using MessagesServer.Hubs;
using MessagesServer.Repositories;

namespace MessagesServer.Services;

public class MessageService(
    IMessageRepository messageRepository,
    IMessageHub messageHub,
    ILogger<MessageService> logger)
    : IMessageService
{
    public async Task SendMessageAsync(MessageToSendDto message)
    {
        var messageDao = message.ToDao();

        messageDao.Timestamp = DateTime.UtcNow;
        logger.LogDebug("Assigned message Timestamp as UTC: {UtcDateTime}", messageDao.Timestamp);
        await messageRepository.AddMessageAsync(messageDao);
        await messageHub.SendMessageAsync(messageDao.Content, messageDao.Timestamp, messageDao.SerialNumber);
    }

    public async Task<List<MessageToGetDto>> GetMessagesAsync(DateTime? from, DateTime? to)
    {
        logger.LogInformation("Choosing interval type for getting messages");

        var messages = (from, to) switch
        {
            (null,              null)            => await messageRepository.GetAllMessagesAsync(),
            (DateTime dateFrom, null)            => await messageRepository.GetMessagesAfterAsync(dateFrom),
            (null,              DateTime dateTo) => await messageRepository.GetMessagesBeforeAsync(dateTo),
            (DateTime dateFrom, DateTime dateTo) => await messageRepository.GetMessagesForPeriodAsync(dateFrom, dateTo)
        };

        return messages.ToDtos();
    }
}
