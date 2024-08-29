using MessageClients.Clients;
using MessageClients.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MessageClients.Controllers;
public class HomeController : Controller
{
    private readonly IMessageClient _messageService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IMessageClient messageService, ILogger<HomeController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(MessageToSendViewModel model)
    {
        if (ModelState.IsValid)
        {
            model.SerialNumber = Guid.NewGuid();
            await _messageService.SendMessageAsync(model);
            ViewBag.Message = "Message sent successfully!";
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
