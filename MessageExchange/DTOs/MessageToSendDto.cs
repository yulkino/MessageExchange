namespace MessageExchange.DTO;

public class MessageToSendDto
{
    public required string Content { get; set; }
    public required Guid SerialNumber { get; set; }
}
