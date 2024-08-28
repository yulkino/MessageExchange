namespace MessageExchange.DAO;

public class MessageDao
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid SerialNumber { get; set; }
}
