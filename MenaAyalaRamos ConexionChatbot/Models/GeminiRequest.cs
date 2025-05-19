using System.ComponentModel.DataAnnotations;

namespace MenaAyalaRamos_ConexionChatbot.Models
{
    public class GeminiRequest
    {
        public List<Content> contents { get; set; }

        public class Part
        {
            public string text { get; set; }
        }

        public class Content
        {
            public List<Part> parts { get; set; }
        }

        [Key]
        public int Id;
    }
}
