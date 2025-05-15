using MenaAyalaRamos_ConexionChatbot.Interfaces;
using MenaAyalaRamos_ConexionChatbot.Models;
using static MenaAyalaRamos_ConexionChatbot.Models.GeminiRequest;
using static System.Net.WebRequestMethods;

namespace MenaAyalaRamos_ConexionChatbot.Repositories
{
    public class GeminiRepository : IChatbotServices
    {
        HttpClient _httpClient;
        private string apiKey = "AIzaSyByx-30z3Zp09aHe33exz7PmMGcKhoUoew";
        public GeminiRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task<string> GetResponse(string prompt)
        {
            string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key="+apiKey;
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, url);

            GeminiRequest request = new GeminiRequest
            {
                contents = new List<Content> 
                {
                    new Content
                    {
                        parts = new List<Part>
                        {
                            new Part
                            {
                                text = prompt
                            }
                        }
                    }
                }
            };
            message.Content = JsonContent.Create<GeminiRequest>(request);
                
            var response = await _httpClient.SendAsync(message);
            string answer = await response.Content.ReadAsStringAsync();

            return answer;
        }
    }
}
    