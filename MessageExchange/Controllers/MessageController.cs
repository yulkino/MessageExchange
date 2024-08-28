using MessageExchange.DTO;
using MessageExchange.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessageExchange.Controllers;
[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{

    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> PostMessage([FromBody] MessageToSendDto message)
    {
        await _messageService.SendMessage(message);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetMessages([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var messages = await _messageService.GetMessages(from, to);
        return Ok(messages);
    }
}
