using MessageClients.Clients;
using MessageClients.Components;
using MessageClients.Options;
using Microsoft.AspNetCore.SignalR.Client;
using SimpleOptions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.AddConfigurationOptions<MessageExchangeOptions>(out var messageExchangeOptions);

builder.Services.AddHttpClient<IMessageClient, MessageClient>(client =>
{
    client.BaseAddress = new Uri(messageExchangeOptions.BaseAddress);
});
builder.Services.AddSignalR();

builder.Services.AddScoped(_ => new HubConnectionBuilder()
            .WithUrl($"{messageExchangeOptions.BaseAddress}/hub/message")
            .WithAutomaticReconnect()
            .Build());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
