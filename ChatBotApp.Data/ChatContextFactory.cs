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
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<ChatContext>();
            optionsBuilder.UseSqlServer("DefaultConnection");

            return new ChatContext(optionsBuilder.Options);
        }
    }
}
