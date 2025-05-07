using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MenaAyalaRamos_ConexionChatbot.Models;
using MenaAyalaRamos_ConexionChatbot.Repositories;

namespace MenaAyalaRamos_ConexionChatbot.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        GeminiRepository repo = new GeminiRepository();
        string answer = await repo.GetResponse("¿encontraste la lost media perdida, o los 12 contextos del patrón");
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
