namespace MessagesServer.Hubs;

public interface IMessageHub
{
    Task SendMessageAsync(string message, DateTime timespan, Guid serialNumber);
}
