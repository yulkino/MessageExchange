using MessageExchange.DAOs;
using Npgsql;

namespace MessageExchange.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<MessageRepository> _logger;

    public MessageRepository(NpgsqlConnection connection, ILogger<MessageRepository> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task AddMessageAsync(MessageDao message)
    {
        if (message.Content.Length > 128)
        {
            _logger.LogWarning("Message exceed 128 characters, length: {ContentLength}", message.Content.Length);
            throw new InvalidOperationException("Message exceed 128 characters");
        }

        var query = "INSERT INTO Messages (Content, Timestamp, SerialNumber) VALUES (@content, @timestamp, @serialNumber)";
        using var cmd = new NpgsqlCommand(query, _connection);

        cmd.Parameters.AddWithValue("content", message.Content);
        cmd.Parameters.AddWithValue("timestamp", message.Timestamp);
        cmd.Parameters.AddWithValue("serialNumber", message.SerialNumber);
        _logger.LogDebug("Parameters to save message in DB: {NpgsqlParameters}", cmd.Parameters);

        try
        {
            await _connection.OpenAsync();
            var result = await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("Query was executed successfully");
        }
        catch(Exception ex)
        {
            _logger.LogError("Query was executed with {Error}", ex);
            throw;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<MessageDao>> GetMessagesForPeriodAsync(DateTime from, DateTime to)
    {
        _logger.LogInformation("Getting messages for interval {From}-{To}", from, to);
        var query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages WHERE Timestamp BETWEEN @from AND @to";
        using var cmd = new NpgsqlCommand(query, _connection);

        cmd.Parameters.AddWithValue("from", from);
        cmd.Parameters.AddWithValue("to", to);
        _logger.LogDebug("Parameters to get message from DB: {NpgsqlParameters}", cmd.Parameters);

        return await ReadMessagesAsync(cmd); ;
    }

    public async Task<List<MessageDao>> GetMessagesAfterAsync(DateTime from)
    {
        _logger.LogInformation("Getting messages for min date: {From}", from);
        var query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages WHERE Timestamp >= @from";

        using var cmd = new NpgsqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("from", from);
        _logger.LogDebug("Parameters to get message from DB: {NpgsqlParameters}", cmd.Parameters);

        return await ReadMessagesAsync(cmd);
    }

    public async Task<List<MessageDao>> GetMessagesBeforeAsync(DateTime to)
    {
        _logger.LogInformation("Getting messages for max date: {To}", to);
        var query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages WHERE Timestamp <= @to";

        using var cmd = new NpgsqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("to", to);
        _logger.LogDebug("Parameters to get message from DB: {NpgsqlParameters}", cmd.Parameters);

        return await ReadMessagesAsync(cmd); ;
    }

    public async Task<List<MessageDao>> GetAllMessagesAsync()
    {
        _logger.LogInformation("Getting all messages");
        var query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages";

        using var cmd = new NpgsqlCommand(query, _connection);

        return await ReadMessagesAsync(cmd);
    }

    private async Task<List<MessageDao>> ReadMessagesAsync(NpgsqlCommand cmd)
    {
        var messages = new List<MessageDao>();

        try
        {
            await _connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                messages.Add(new MessageDao
                {
                    Id = reader.GetGuid(0),
                    Content = reader.GetString(1),
                    Timestamp = reader.GetDateTime(2),
                    SerialNumber = reader.GetGuid(3)
                });
            }

            _logger.LogInformation("Messages received successfully");
        }
        catch(Exception ex)
        {
            _logger.LogError("Messages received with error: {Error}", ex);
            throw;
        }
        finally
        {
            await _connection.CloseAsync();
        }

        _logger.LogDebug("Got {MessageCount} messages", messages.Count);
        return messages;
    }
}
