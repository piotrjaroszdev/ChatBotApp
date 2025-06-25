using ChatBotApp.Data.Models;
using ChatBotApp.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMessageRepository _repository;

        public ChatController(IMessageRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory() =>
            Ok(await _repository.GetHistoryAsync());

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] Message userMessage)
        {
            var botReply = await _repository.AddUserAndBotMessageAsync(userMessage);
            return Ok(botReply);
        }

        [HttpPost("rate")]
        public async Task<IActionResult> RateMessage(int messageId, int rating)
        {
            var updated = await _repository.RateMessageAsync(messageId, rating);
            if (!updated) return NotFound();
            return Ok();
        }
    }
}
