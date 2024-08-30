using MessageClients.Clients;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

var serverUrl = "http://server:8080";
services.AddHttpClient<IMessageClient, MessageClient>(client =>
{
    client.BaseAddress = new Uri(serverUrl);
});

services.AddSingleton(provider =>
{
    var connection = new HubConnectionBuilder()
        .WithUrl($"http://server:8080/hub/message")
        .WithAutomaticReconnect()
        .Build();

    connection.StartAsync().GetAwaiter().GetResult();
    return connection;
});

services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
