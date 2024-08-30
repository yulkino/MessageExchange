using AutoMapper;
using MessageExchange.DAO;
using MessageExchange.DTO;
using MessageExchange.DTOs;
using MessageExchange.Hubs;
using MessageExchange.Repositories;

namespace MessageExchange.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMessageHub _messageHub;
    private readonly IMapper _mapper;

    public MessageService(IMessageRepository messageRepository, IMessageHub messageHub, IMapper mapper)
    {
        _messageRepository = messageRepository;
        _messageHub = messageHub;
        _mapper = mapper;
    }

    public async Task SendMessage(MessageToSendDto message)
    {
        var messageDao = _mapper.Map<MessageDao>(message);

        messageDao.Timestamp = DateTime.UtcNow;
        await _messageRepository.AddMessageAsync(messageDao);
        await _messageHub.SendMessageToAll(messageDao.Content, messageDao.Timestamp, messageDao.SerialNumber);
    }

    public async Task<List<MessageToGetDto>> GetMessages(DateTime? from, DateTime? to)
    {
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
