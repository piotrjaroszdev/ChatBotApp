using ChatBotApp.Data.ChatBotApp.Api.Data;
using ChatBotApp.Data.Models;
using ChatBotApp.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ChatBotApp.Tests
{
    public class ChatControllerTests
    {
        private static Mock<DbSet<Message>> CreateMockDbSet(List<Message> data)
        {
            var queryable = data.AsQueryable();

            var mockSet = new Mock<DbSet<Message>>();
            mockSet.As<IQueryable<Message>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Message>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Message>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Message>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            mockSet.Setup(d => d.Add(It.IsAny<Message>())).Callback<Message>(data.Add);

            mockSet.Setup(d => d.FindAsync(It.IsAny<object[]>()))
                .Returns<object[]>(ids =>
                {
                    var id = (int)ids[0];
                    var entity = data.FirstOrDefault(m => m.Id == id);
                    return new ValueTask<Message>(entity);
                });

            return mockSet;
        }

        [Fact]
        public async Task SendMessage_AddsUserAndBotMessage()
        {
            // Arrange
            var messages = new List<Message>();
            var mockSet = CreateMockDbSet(messages);

            var mockContext = new Mock<ChatContext>(new DbContextOptions<ChatContext>());
            mockContext.Setup(c => c.Messages).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var controller = new ChatController(mockContext.Object);
            var userMessage = new Message { Sender = "user", Content = "Test", Timestamp = System.DateTime.UtcNow };

            // Act
            var result = await controller.SendMessage(userMessage);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var botReply = Assert.IsType<Message>(okResult.Value);
            Assert.Equal("bot", botReply.Sender);
            Assert.Equal(2, messages.Count);
        }

        [Fact]
        public async Task RateMessage_UpdatesRating()
        {
            // Arrange
            var message = new Message { Id = 1, Sender = "user", Content = "Test", Timestamp = System.DateTime.UtcNow };
            var messages = new List<Message> { message };
            var mockSet = CreateMockDbSet(messages);

            var mockContext = new Mock<ChatContext>(new DbContextOptions<ChatContext>());
            mockContext.Setup(c => c.Messages).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var controller = new ChatController(mockContext.Object);

            // Act
            var result = await controller.RateMessage(1, 5);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(5, message.Rating);
        }

        [Fact]
        public async Task RateMessage_ReturnsNotFound_WhenMessageDoesNotExist()
        {
            // Arrange
            var messages = new List<Message>();
            var mockSet = CreateMockDbSet(messages);

            var mockContext = new Mock<ChatContext>(new DbContextOptions<ChatContext>());
            mockContext.Setup(c => c.Messages).Returns(mockSet.Object);

            var controller = new ChatController(mockContext.Object);

            // Act
            var result = await controller.RateMessage(999, 5);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
