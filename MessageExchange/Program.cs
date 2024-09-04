using MessageExchange;
using MessageExchange.Hubs;
using MessageExchange.Repositories;
using MessageExchange.Services;
using Microsoft.AspNetCore.SignalR;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddSingleton(new NpgsqlConnection(connectionString));

services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IMessageHub, MessageHub>();
services.AddAutoMapper(AssemblyMarker.Assembly);
services.AddSignalR();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MessageHub>("/hub/message");

app.Run();
