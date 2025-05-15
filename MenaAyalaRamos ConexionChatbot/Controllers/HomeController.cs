using MenaAyalaRamos_ConexionChatbot.Interfaces;
using MenaAyalaRamos_ConexionChatbot.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IChatbotServices _chatbotServices;

    public HomeController(ILogger<HomeController> logger, IChatbotServices chatbotServices)
    {
        _logger = logger;
        _chatbotServices = chatbotServices;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return View();
        }

        string answer = await _chatbotServices.GetResponse(prompt);
        ViewBag.Respuesta = answer;
        return View();
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
