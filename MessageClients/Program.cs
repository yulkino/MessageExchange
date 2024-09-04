using MessageClients.Clients;
using MessageClients.Components;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IMessageClient, MessageClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5108");
});
builder.Services.AddSignalR();

builder.Services.AddScoped(_ => new HubConnectionBuilder()
            .WithUrl($"http://localhost:5108/hub/message")
            .WithAutomaticReconnect()
            .Build());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
