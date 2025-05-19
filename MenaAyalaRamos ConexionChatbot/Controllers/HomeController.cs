using MenaAyalaRamos_ConexionChatbot.Data;
using MenaAyalaRamos_ConexionChatbot.Interfaces;
using MenaAyalaRamos_ConexionChatbot.Models;
using MenaAyalaRamos_ConexionChatbot.Models.MenaAyalaRamos_ConexionChatbot.Models;
using MenaAyalaRamos_ConexionChatbot.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IChatbotServices _chatbotServices;
    private readonly ChatDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, IChatbotServices chatbotServices, ChatDbContext dbContext)
    {
        _logger = logger;
        _chatbotServices = chatbotServices;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string prompt, string chatbot)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            ViewBag.Respuesta = "Por favor, ingresa una pregunta.";
            return View();
        }

        string rawResponse = "";
        string respuestaSimple = "";

        try
        {
            if (chatbot == "TogetherAI")
            {
                var chatTogetherAIService = new TogetherAIRepository();
                rawResponse = await chatTogetherAIService.GetResponse(prompt);

                using JsonDocument doc = JsonDocument.Parse(rawResponse);
                var root = doc.RootElement;

                if (root.TryGetProperty("choices", out JsonElement choices))
                {
                    respuestaSimple = choices[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString() ?? "No se obtuvo respuesta";
                    var chat = new ChatInteraction
                    {
                        Prompt = prompt,
                        Response = respuestaSimple,
                        Provider = "TogetherAI",
                        TokensUsed = root.GetProperty("usage").GetProperty("total_tokens").GetInt32(),
                        Timestamp = DateTime.Now
                    };
                    _dbContext.ChatInteractions.Add(chat);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    respuestaSimple = "Estructura de respuesta desconocida.";
                }
            }
            else
            {
                rawResponse = await _chatbotServices.GetResponse(prompt);

                using JsonDocument doc = JsonDocument.Parse(rawResponse);
                var root = doc.RootElement;

                if (root.TryGetProperty("candidates", out JsonElement candidates))
                {
                    respuestaSimple = candidates[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString() ?? "No se obtuvo respuesta";
                    var chat = new ChatInteraction
                    {
                        Prompt = prompt,
                        Response = respuestaSimple,
                        Provider = "Gemini",
                        TokensUsed = root.GetProperty("usage").GetProperty("total_tokens").GetInt32(),
                        Timestamp = DateTime.Now
                    };
                    _dbContext.ChatInteractions.Add(chat);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    respuestaSimple = "Estructura de respuesta desconocida.";
                }
            }
        }
        catch (Exception ex)
        {
            respuestaSimple = $"Error al procesar la respuesta: {ex.Message}";
        }



        ViewBag.Respuesta = respuestaSimple;
        ViewBag.ChatbotSeleccionado = chatbot;

        return View();
    }
}
