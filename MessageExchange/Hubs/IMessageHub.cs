namespace MessageExchange.Hubs;

public interface IMessageHub
{
    Task SendMessageToAll(string message, DateTime timespan, Guid serialNumber);
}
