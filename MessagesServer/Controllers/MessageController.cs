using MessagesServer.DTOs;
using MessagesServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessagesServer.Controllers;
[ApiController]
[Route("[controller]")]
public class MessageController(IMessageService messageService, ILogger<MessageController> logger) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> PostMessage([FromBody] MessageToSendDto message)
    {
        logger.LogInformation("Message to post: {Message}", message);
        await messageService.SendMessageAsync(message);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<MessageToGetDto>>> GetMessages([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        logger.LogInformation("Given interval: {From}-{To}", from, to);
        var messages = await messageService.GetMessagesAsync(from, to);
        return Ok(messages);
    }
}
