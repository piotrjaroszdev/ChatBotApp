namespace ChatBotApp.Data
{
    using global::ChatBotApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    namespace ChatBotApp.Api.Data
    {
        public class ChatContext : DbContext
        {
            public ChatContext(DbContextOptions<ChatContext> options) : base(options)
            {
            }

            public virtual DbSet<Message> Messages { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Message>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Sender)
                          .IsRequired()
                          .HasMaxLength(50);
                    entity.Property(e => e.Content)
                          .IsRequired();
                    entity.Property(e => e.Timestamp)
                          .IsRequired();
                    entity.Property(e => e.Rating);
                });
            }
        }
    }

}
