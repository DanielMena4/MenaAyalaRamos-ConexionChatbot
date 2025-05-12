using MenaAyalaRamos_ConexionChatbot.Interfaces;

namespace MenaAyalaRamos_ConexionChatbot.Repositories
{
    public class DeepSeekRepository : IChatbotServices
    {
        public async Task<string> GetResponse(string prompt)
        {
            return "Esto es la respuesta de DeepSeek";
        }
    }
}
