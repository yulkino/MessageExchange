namespace MessageExchange.DTOs;

public class MessageToGetDto
{
    public required string Message { get; set; }
    public required DateTime Timestamp { get; set; }
    public required Guid SerialNumber { get; set; }

    public override string ToString() => 
        $"Message: {Message}; Timestamp: {Timestamp}; (SerialNumber: {SerialNumber})";
}
