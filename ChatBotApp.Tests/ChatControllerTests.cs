using ChatBotApp.Data.Models;
using ChatBotApp.Server.Controllers;
using ChatBotApp.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ChatBotApp.Tests
{
    public class ChatControllerTests
    {
        [Fact]
        public async Task GetHistory_ReturnsAllMessages()
        {
            // Arrange
            var messages = new List<Message>
            {
                new Message { Id = 1, Sender = "user", Content = "Hi" },
                new Message { Id = 2, Sender = "bot", Content = "Hello" }
            };
            var mockRepo = new Mock<IMessageRepository>();
            mockRepo.Setup(r => r.GetHistoryAsync()).ReturnsAsync(messages);

            var controller = new ChatController(mockRepo.Object);

            // Act
            var result = await controller.GetHistory();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultMessages = Assert.IsAssignableFrom<IEnumerable<Message>>(okResult.Value);
            Assert.Equal(2, System.Linq.Enumerable.Count(resultMessages));
        }

        [Fact]
        public async Task SendMessage_AddsUserAndBotMessage()
        {
            // Arrange
            var userMessage = new Message { Id = 1, Sender = "user", Content = "Test" };
            var botReply = new Message { Id = 2, Sender = "bot", Content = "Bot reply" };

            var mockRepo = new Mock<IMessageRepository>();
            mockRepo.Setup(r => r.AddUserAndBotMessageAsync(userMessage)).ReturnsAsync(botReply);

            var controller = new ChatController(mockRepo.Object);

            // Act
            var result = await controller.SendMessage(userMessage);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBotReply = Assert.IsType<Message>(okResult.Value);
            Assert.Equal("bot", returnedBotReply.Sender);
            Assert.Equal("Bot reply", returnedBotReply.Content);
        }

        [Fact]
        public async Task RateMessage_UpdatesRating()
        {
            // Arrange
            var mockRepo = new Mock<IMessageRepository>();
            mockRepo.Setup(r => r.RateMessageAsync(1, 5)).ReturnsAsync(true);

            var controller = new ChatController(mockRepo.Object);

            // Act
            var result = await controller.RateMessage(1, 5);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RateMessage_ReturnsNotFound_WhenMessageDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<IMessageRepository>();
            mockRepo.Setup(r => r.RateMessageAsync(999, 5)).ReturnsAsync(false);

            var controller = new ChatController(mockRepo.Object);

            // Act
            var result = await controller.RateMessage(999, 5);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
