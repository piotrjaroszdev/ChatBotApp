using ChatBotApp.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatBotApp.Data.Repositories
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetHistoryAsync();
        Task<Message> AddUserAndBotMessageAsync(Message userMessage);
        Task<bool> RateMessageAsync(int messageId, int rating);
    }
}