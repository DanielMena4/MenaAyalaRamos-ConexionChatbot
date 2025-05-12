namespace MenaAyalaRamos_ConexionChatbot.Interfaces
{
    public interface IChatbotServices
    {
        public Task<string> GetResponse(string prompt);
    }
}
