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

            if (root.TryGetProperty("candidates", out JsonElement candidates))
            {
                respuestaSimple = candidates[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString() ?? "No se obtuvo respuesta";
            }
            else if (root.TryGetProperty("choices", out JsonElement choices))
            {
                respuestaSimple = choices[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "No se obtuvo respuesta";
            }
            else
            {
                respuestaSimple = "Estructura de respuesta desconocida.";
            }
        }
        catch (Exception ex)
        {
            respuestaSimple = $"Error al procesar la respuesta: {ex.Message}";
        }
        ViewBag.Respuesta = respuestaSimple;
        return View();
    }
}

