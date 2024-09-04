namespace MessageExchange.DTOs;

public class MessageToGetDto
{
    public required string Message { get; set; }
    public required DateTime Timestamp { get; set; }
    public required Guid SerialNumber { get; set; }
}
