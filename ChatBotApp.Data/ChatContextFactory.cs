using ChatBotApp.Data.ChatBotApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
namespace ChatBotApp.Data
{
    public class ChatContextFactory : IDesignTimeDbContextFactory<ChatContext>
    {
        public ChatContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=ChatBotDb;Trusted_Connection=True;TrustServerCertificate=True;");
            return new ChatContext(optionsBuilder.Options);
        }
    }
}
