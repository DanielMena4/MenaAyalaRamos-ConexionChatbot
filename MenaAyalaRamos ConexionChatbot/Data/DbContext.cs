using Microsoft.EntityFrameworkCore;
using MenaAyalaRamos_ConexionChatbot.Models;
using MenaAyalaRamos_ConexionChatbot.Models.MenaAyalaRamos_ConexionChatbot.Models;
using System.Collections.Generic;

namespace MenaAyalaRamos_ConexionChatbot.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChatInteraction> ChatInteractions { get; set; }
    }
}
