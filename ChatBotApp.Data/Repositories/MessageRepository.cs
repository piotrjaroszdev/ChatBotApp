using ChatBotApp.Data.ChatBotApp.Api.Data;
using ChatBotApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotApp.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ChatContext _context;

        public MessageRepository(ChatContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetHistoryAsync()
        {
            return await _context.Messages.OrderBy(m => m.Timestamp).ToListAsync();
        }

        public async Task<Message> AddUserAndBotMessageAsync(Message userMessage)
        {
            _context.Messages.Add(userMessage);

            var botReply = new Message
            {
                Sender = "bot",
                Content = GenerateFakeReply(),
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(botReply);
            await _context.SaveChangesAsync();

            return botReply;
        }

        public async Task<bool> RateMessageAsync(int messageId, int rating)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message == null) return false;

            message.Rating = rating;
            await _context.SaveChangesAsync();
            return true;
        }

        private string GenerateFakeReply()
        {
            string[] responses = {
                "Fajny pomys³",
                "To jest niesamowite",
                "Potrzebujê wiêcej informacji",
                "Ciekawa myœl, rozwiniesz?",
                "Brzmi intryguj¹co!",
                "Mo¿esz opowiedzieæ coœ wiêcej?"
            };
            return responses[new Random().Next(responses.Length)];
        }
    }
}