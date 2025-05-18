using MenaAyalaRamos_ConexionChatbot.Interfaces;
using MenaAyalaRamos_ConexionChatbot.Models;
using static MenaAyalaRamos_ConexionChatbot.Models.TogetherAiRequest;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MenaAyalaRamos_ConexionChatbot.Repositories
{
    public class TogetherAIRepository : IChatbotServices
    {
        private readonly HttpClient _httpClient;
        private string apiKey = "4347dbe12518ffe873ab8bef5750e87326788ece51878cdb43511513d190967e"; 

        public TogetherAIRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetResponse(string prompt)
        {
            string url = "https://api.together.xyz/v1/chat/completions";

            TogetherAiRequest request = new TogetherAiRequest
            {
                model = "meta-llama/Llama-4-Maverick-17B-128E-Instruct-FP8",
                messages = new List<Message>
                {
                    new Message
                    {
                        role = "user",
                        content = prompt
                    }
                }
            };

            var json = JsonSerializer.Serialize(request);
            var message = new HttpRequestMessage(HttpMethod.Post, url);
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(message);
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
