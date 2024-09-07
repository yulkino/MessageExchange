using MessagesServer.DAOs;
using Npgsql;

namespace MessagesServer.Repositories;

public class MessageRepository(NpgsqlConnection connection, ILogger<MessageRepository> logger) : IMessageRepository
{
    public async Task AddMessageAsync(MessageDao message)
    {
        if (message.Content.Length > 128)
        {
            var ex = new InvalidOperationException("Message exceed 128 characters");
            logger.LogError(ex, "Message exceed 128 characters, length: {ContentLength}", message.Content.Length);
            throw ex;
        }

        const string query = "INSERT INTO Messages (Content, Timestamp, SerialNumber) VALUES (@content, @timestamp, @serialNumber)";
        await using var cmd = new NpgsqlCommand(query, connection);

        cmd.Parameters.AddWithValue("content", message.Content);
        cmd.Parameters.AddWithValue("timestamp", message.Timestamp);
        cmd.Parameters.AddWithValue("serialNumber", message.SerialNumber);
        logger.LogDebug("Parameters to save message in DB: {@NpgsqlParameters}", cmd.Parameters);

        try
        {
            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            logger.LogInformation("Query was executed successfully");
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Unable to add message");
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<List<MessageDao>> GetMessagesForPeriodAsync(DateTime from, DateTime to)
    {
        logger.LogInformation("Getting messages for interval {From}-{To}", from, to);
        const string query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages WHERE Timestamp BETWEEN @from AND @to";
        await using var cmd = new NpgsqlCommand(query, connection);

        cmd.Parameters.AddWithValue("from", from);
        cmd.Parameters.AddWithValue("to", to);
        logger.LogDebug("Parameters to get message from DB: {@NpgsqlParameters}", cmd.Parameters);

        return await ReadMessagesAsync(cmd); ;
    }

    public async Task<List<MessageDao>> GetMessagesAfterAsync(DateTime from)
    {
        logger.LogInformation("Getting messages for min date: {From}", from);
        const string query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages WHERE Timestamp >= @from";

        await using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("from", from);
        logger.LogDebug("Parameters to get message from DB: {@NpgsqlParameters}", cmd.Parameters);

        return await ReadMessagesAsync(cmd);
    }

    public async Task<List<MessageDao>> GetMessagesBeforeAsync(DateTime to)
    {
        logger.LogInformation("Getting messages for max date: {To}", to);
        const string query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages WHERE Timestamp <= @to";

        await using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("to", to);
        logger.LogDebug("Parameters to get message from DB: {@NpgsqlParameters}", cmd.Parameters);

        return await ReadMessagesAsync(cmd); ;
    }

    public async Task<List<MessageDao>> GetAllMessagesAsync()
    {
        logger.LogInformation("Getting all messages");
        const string query = "SELECT Id, Content, Timestamp, SerialNumber FROM Messages";

        await using var cmd = new NpgsqlCommand(query, connection);

        return await ReadMessagesAsync(cmd);
    }

    private async Task<List<MessageDao>> ReadMessagesAsync(NpgsqlCommand cmd)
    {
        var messages = new List<MessageDao>();

        try
        {
            await connection.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
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

            logger.LogInformation("Messages received successfully");
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Unable to receive messages");
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }

        logger.LogDebug("Got {MessageCount} messages", messages.Count);
        return messages;
    }


    public async Task EnsureTableExistsAsync()
    {
        try
        {
            await connection.OpenAsync();

            const string createTableQuery = @"
            CREATE EXTENSION IF NOT EXISTS pgcrypto;
            
            CREATE TABLE IF NOT EXISTS Messages (
                Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                Content TEXT NOT NULL,
                Timestamp TIMESTAMP NOT NULL,
                SerialNumber UUID NOT NULL
            );";

            await using var command = new NpgsqlCommand(createTableQuery, connection);
            await command.ExecuteNonQueryAsync();
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Unable to create Messages table");
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}
