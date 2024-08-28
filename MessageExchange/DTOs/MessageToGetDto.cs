namespace MessageExchange.DTOs;

public class MessageToGetDto
{
    public required string Content { get; set; }
    public required DateTime Timestamp { get; set; }
    public required Guid SerialNumber { get; set; }
}
