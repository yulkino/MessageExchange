using MessageClients.Clients;
using MessageClients.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MessageClients.Controllers;

public class MessagesController : Controller
{
    private readonly IMessageClient _messageClient;

    public MessagesController(IMessageClient messageClient)
    {
        _messageClient = messageClient;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = new MessageFilterViewModel
        {
            Messages = new List<MessageToGetViewModel>()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(MessageFilterViewModel filter)
    {
        var messages = new List<MessageToGetViewModel>();

        var IsParsed = int.TryParse(filter.TimeRange, out var minutes);
        if (IsParsed)
        {
            var currentDate = DateTime.UtcNow;
            filter.DateTimeFrom = currentDate.AddMinutes(-minutes);
            filter.DateTimeTo = currentDate;
        }
        else if (filter.TimeRange == "all")
        {
            filter.DateTimeFrom = null;
            filter.DateTimeTo = null;
        }
                
        messages = await _messageClient.GetMessagesAsync(filter.DateTimeFrom, filter.DateTimeTo);

        filter.Messages = messages;
        return View(filter);
    }
}
