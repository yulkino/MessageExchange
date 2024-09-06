using MessageExchange.DTOs;
using MessageExchange.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessageExchange.Controllers;
[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ILogger<MessageController> _logger;

    public MessageController(IMessageService messageService, ILogger<MessageController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> PostMessage([FromBody] MessageToSendDto message)
    {
        _logger.LogInformation("Message to post: {Message}", message);
        await _messageService.SendMessage(message);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<MessageToGetDto>>> GetMessages([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        _logger.LogInformation("Given interval: {From}-{To}", from, to);
        var messages = await _messageService.GetMessages(from, to);
        return Ok(messages);
    }
}
