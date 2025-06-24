using ChatBotApp.Data.ChatBotApp.Api.Data;
using ChatBotApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatContext _context;

        public ChatController(ChatContext context)
        {
            _context = context;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory() =>
            Ok(await _context.Messages.OrderBy(m => m.Timestamp).ToListAsync());

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] Message userMessage)
        {
            _context.Messages.Add(userMessage);

            var botReply = new Message
            {
                Sender = "bot",
                Content = GenerateFakeReply(), // lub "Lorem ipsum..."
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(botReply);
            await _context.SaveChangesAsync();

            return Ok(botReply);
        }

        private string GenerateFakeReply()
        {
            string[] responses = {
            "Lorem ipsum dolor sit amet.",
            "Ciekawa myśl, rozwiniesz?",
            "Brzmi intrygująco!",
            "Możesz opowiedzieć coś więcej?"
        };
            return responses[new Random().Next(responses.Length)];
        }

        [HttpPost("rate")]
        public async Task<IActionResult> RateMessage(int messageId, int rating)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message == null) return NotFound();

            message.Rating = rating;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }

}
