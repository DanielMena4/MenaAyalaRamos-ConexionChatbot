    using MenaAyalaRamos_ConexionChatbot.Interfaces;
    using MenaAyalaRamos_ConexionChatbot.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
using System.Text.Json;

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

        string rawResponse = await _chatbotServices.GetResponse(prompt);

        string respuestaSimple;

        try
        {
            using JsonDocument doc = JsonDocument.Parse(rawResponse);
            var root = doc.RootElement;

            respuestaSimple = root.GetProperty("candidates")[0]
                                 .GetProperty("content")
                                 .GetProperty("parts")[0]
                                 .GetProperty("text")
                                 .GetString() ?? "No se obtuvo respuesta";
        }
        catch
        {
            respuestaSimple = "Error al procesar la respuesta.";
        }

        ViewBag.Respuesta = respuestaSimple;
        return View();
    }
}

