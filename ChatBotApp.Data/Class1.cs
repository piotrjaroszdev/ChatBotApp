using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApp.Data
{
    public class Class1
    {
        public class ChatContext : DbContext
        {
            public ChatContext(DbContextOptions<ChatContext> options) : base(options)
            {
            }
            public DbSet<Message> Messages { get; set; }

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
                    entity.Property(e => e.Rating)
                          .HasDefaultValue(null);
                });
            }
        }

    }
}
