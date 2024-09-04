namespace MessageExchange.DTO;

public class MessageToSendDto
{
    public required string Message { get; set; }
    public required Guid SerialNumber { get; set; }
}
