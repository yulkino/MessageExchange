using AutoMapper;
using MessageExchange.DAOs;
using MessageExchange.DTOs;
using MessageExchange.Hubs;
using MessageExchange.Repositories;

namespace MessageExchange.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMessageHub _messageHub;
    private readonly IMapper _mapper;
    private readonly ILogger<MessageService> _logger;

    public MessageService(IMessageRepository messageRepository, IMessageHub messageHub,
        IMapper mapper, ILogger<MessageService> logger)
    {
        _messageRepository = messageRepository;
        _messageHub = messageHub;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task SendMessage(MessageToSendDto message)
    {
        var messageDao = _mapper.Map<MessageDao>(message);

        messageDao.Timestamp = DateTime.UtcNow;
        _logger.LogDebug("Assigned message Timestamp as UTC: {UtcDateTime}", messageDao.Timestamp);
        await _messageRepository.AddMessageAsync(messageDao);
        await _messageHub.SendMessageToAll(messageDao.Content, messageDao.Timestamp, messageDao.SerialNumber);
    }

    public async Task<List<MessageToGetDto>> GetMessages(DateTime? from, DateTime? to)
    {
        _logger.LogInformation("Choosing interval type for getting messages");
        List<MessageDao> messages;

        messages = (from, to) switch
        {
            (null,              null)            => await _messageRepository.GetAllMessagesAsync(),
            (DateTime dateFrom, null)            => await _messageRepository.GetMessagesAfterAsync(dateFrom),
            (null,              DateTime dateTo) => await _messageRepository.GetMessagesBeforeAsync(dateTo),
            (DateTime dateFrom, DateTime dateTo) => await _messageRepository.GetMessagesForPeriodAsync(dateFrom, dateTo)
        };

        var messagesDto = _mapper.Map<List<MessageToGetDto>>(messages);
        return messagesDto;
    }
}
