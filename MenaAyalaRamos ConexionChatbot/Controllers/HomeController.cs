using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MenaAyalaRamos_ConexionChatbot.Models;
using MenaAyalaRamos_ConexionChatbot.Repositories;
using MenaAyalaRamos_ConexionChatbot.Interfaces;

namespace MenaAyalaRamos_ConexionChatbot.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IChatbotServices _chatbotServices;

    public HomeController(ILogger<HomeController> logger, IChatbotServices chatbotService)
    {
        _logger = logger;
        _chatbotServices = chatbotService;
    }

    public async Task<IActionResult> Index()
    {
        string answer = await _chatbotServices.GetResponse("hola");
        return View(answer);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
