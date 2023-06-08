using JTM.CQRS.Command.Account;
using JTM.Data;
using JTM.DTO.Account.RegisterUser;
using JTM.Enum;
using JTM.Exceptions;
using JTM.Services.RabbitService;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace JTM.IntegrationTests.CQRS.Command.Account
{
    public class ForgetPasswordCommandTests
    {

        private readonly DataContext _dataContext;
        private readonly Mock<IRabbitService> _mockRabbitService = new();

        public ForgetPasswordCommandTests()
        {
            DbContextOptions<DataContext> contextOptions = new DbContextOptionsBuilder<DataContext>()
               .UseInMemoryDatabase(databaseName: "InMemory_JTM")
               .Options;
            _dataContext = new DataContext(contextOptions);
        }

        [Fact]
        public async Task ForgetPassword_ForNotExitsingUser_ShouldThrowsAuthExeptionWithUserNotFoundMessage()
        {
            // Arrange
            _mockRabbitService.Setup(c => c.SendMessage(It.IsAny<MessageQueueType>(), It.IsAny<MessageDto>()));
            var command = new ForgetPasswordCommand("");
            var commandHandler = new ForgetPasswordUserCommandHandler(_dataContext, _mockRabbitService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        //[Fact]
        //public async Task ForgetPassword_ForExistingUser_Should
    }
}
