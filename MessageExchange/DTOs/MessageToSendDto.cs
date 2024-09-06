namespace MessageExchange.DTOs;

public class MessageToSendDto
{
    public required string Message { get; set; }
    public required Guid SerialNumber { get; set; }

    public override string ToString() => 
        $"Message: {Message}; (SerialNumber: {SerialNumber})";
}
