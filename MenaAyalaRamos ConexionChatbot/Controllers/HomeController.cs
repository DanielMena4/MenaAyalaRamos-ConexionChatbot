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
        int tokens = 0;

        try
        {
            if (chatbot == "TogetherAI")
            {
                var chatTogetherAIService = new TogetherAIRepository();
                rawResponse = await chatTogetherAIService.GetResponse(prompt);

                using JsonDocument doc = JsonDocument.Parse(rawResponse);
                var root = doc.RootElement;

                if (root.TryGetProperty("choices", out JsonElement choices) &&
                    choices.GetArrayLength() > 0)
                {
                    respuestaSimple = choices[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString() ?? "No se obtuvo respuesta";

                    if (root.TryGetProperty("usage", out JsonElement usage) &&
                        usage.TryGetProperty("total_tokens", out JsonElement totalTokens))
                    {
                        tokens = totalTokens.GetInt32();
                    }

                    _dbContext.ChatInteractions.Add(new ChatInteraction
                    {
                        Prompt = prompt,
                        Response = respuestaSimple,
                        Provider = "TogetherAI",
                        TokensUsed = tokens,
                        Timestamp = DateTime.Now
                    });
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    respuestaSimple = "Estructura de respuesta desconocida (TogetherAI).";
                }
            }
            else 
            {
                rawResponse = await _chatbotServices.GetResponse(prompt);

                using JsonDocument doc = JsonDocument.Parse(rawResponse);
                var root = doc.RootElement;

                if (root.TryGetProperty("candidates", out JsonElement candidates) &&
                    candidates.GetArrayLength() > 0 &&
                    candidates[0].TryGetProperty("content", out JsonElement content) &&
                    content.TryGetProperty("parts", out JsonElement parts) &&
                    parts.GetArrayLength() > 0 &&
                    parts[0].TryGetProperty("text", out JsonElement texto))
                {
                    respuestaSimple = texto.GetString() ?? "No se obtuvo respuesta";
                }
                else
                {
                    respuestaSimple = "Estructura de respuesta desconocida (Gemini).";
                }
                _dbContext.ChatInteractions.Add(new ChatInteraction
                {
                    Prompt = prompt,
                    Response = respuestaSimple,
                    Provider = "Gemini",
                    TokensUsed = 0,
                    Timestamp = DateTime.Now
                });
                await _dbContext.SaveChangesAsync();
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
