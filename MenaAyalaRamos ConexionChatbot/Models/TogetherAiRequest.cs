namespace MenaAyalaRamos_ConexionChatbot.Models
{
    public class TogetherAiRequest
    {
        public string model { get; set; }
        public List<Message> messages { get; set; }

        public class Message
        {
            public string role { get; set; }
            public string content { get; set; }
        }

    }
}
