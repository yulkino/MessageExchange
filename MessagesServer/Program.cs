using MessagesServer.Hubs;
using MessagesServer.Repositories;
using MessagesServer.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var services = builder.Services;

services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddSingleton(new NpgsqlConnection(connectionString));

services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IMessageHub, MessageHub>();
services.AddSignalR();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MessageHub>("/hub/message");

using var scope = app.Services.CreateScope();
var messagesRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
await messagesRepository.EnsureTableExistsAsync();

app.Run();
