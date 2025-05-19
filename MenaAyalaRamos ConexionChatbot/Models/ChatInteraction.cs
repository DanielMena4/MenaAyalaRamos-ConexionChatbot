using System;
using System.ComponentModel.DataAnnotations;

namespace MenaAyalaRamos_ConexionChatbot.Models
{
    namespace MenaAyalaRamos_ConexionChatbot.Models
    {
        public class ChatInteraction
        {
            [Key]
            public int Id { get; set; }

            public string Prompt { get; set; }
            public string Response { get; set; }
            public string Provider { get; set; }
            public int TokensUsed { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }

}
