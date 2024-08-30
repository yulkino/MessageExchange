using MessageClients.Clients;
using MessageClients.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageClients.Controllers;

public class SocketMessagesController : Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        var model = new SocketMessagesViewModel();
        return View(model);
    }
}
