namespace MessageExchange.DAOs;

public class MessageDao
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid SerialNumber { get; set; }

    public override string ToString() =>
        $"Id: {Id}; Content: {Content}; Timestamp: {Timestamp}; (SerialNumber: {SerialNumber})";
}
