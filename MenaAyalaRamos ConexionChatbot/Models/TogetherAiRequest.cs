namespace MenaAyalaRamos_ConexionChatbot.Models
{
    public class TogetherAiRequest
    {
        public string model { get; set; }
        public List<message> messages { get; set; }

        public class message
        {
            public string role { get; set; }
            public string content { get; set; }
        }

    }
}
