using MessageExchange.DAO;
using Npgsql;

namespace MessageExchange.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly NpgsqlConnection _connection;

    public MessageRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task AddMessageAsync(MessageDao message)
    {
        var query = "INSERT INTO Messages (Content, Timestamp, SerialNumber) VALUES (@content, @timestamp, @serialNumber)";
        using var cmd = new NpgsqlCommand(query, _connection);

        cmd.Parameters.AddWithValue("content", message.Content);
        cmd.Parameters.AddWithValue("timestamp", message.Timestamp);
        cmd.Parameters.AddWithValue("serialNumber", message.SerialNumber);

        await _connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
        await _connection.CloseAsync();
    }

    public async Task<List<MessageDao>> GetMessagesForPeriodAsync(DateTime from, DateTime to)
    {
        var query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages WHERE Timestamp BETWEEN @from AND @to";

        using var cmd = new NpgsqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("from", from);
        cmd.Parameters.AddWithValue("to", to);

        return await ReadMessagesAsync(cmd); ;
    }

    public async Task<List<MessageDao>> GetMessagesAfterAsync(DateTime from)
    {
        var query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages WHERE Timestamp >= @from";

        using var cmd = new NpgsqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("from", from);

        return await ReadMessagesAsync(cmd);
    }

    public async Task<List<MessageDao>> GetMessagesBeforeAsync(DateTime to)
    {
        var query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages WHERE Timestamp <= @to";

        using var cmd = new NpgsqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("to", to);

        return await ReadMessagesAsync(cmd); ;
    }

    public async Task<List<MessageDao>> GetAllMessagesAsync()
    {
        var query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages";

        using var cmd = new NpgsqlCommand(query, _connection);

        return await ReadMessagesAsync(cmd);
    }

    private async Task<List<MessageDao>> ReadMessagesAsync(NpgsqlCommand cmd)
    {
        var messages = new List<MessageDao>();

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
        await _connection.CloseAsync();

        return messages;
    }
}
