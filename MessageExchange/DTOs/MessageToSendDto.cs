using System.ComponentModel.DataAnnotations;

namespace MessageExchange.DTOs;

public class MessageToSendDto
{
    public required string Message { get; set; }
    public required Guid SerialNumber { get; set; }
}
