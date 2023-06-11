using FluentValidation;
using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.DTO.Account.RegisterUser;
using JTM.Enum;
using Moq;
using System.Linq.Expressions;

namespace JTM.IntegrationTests.CQRS_Tests.Command.Account
{
    public class RegisterCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task RegisterUser_ForBusyEmail_ShouldThrowValidationExceptionWithBusyMessageAsync()
        {
            // Arrange
            MockRabbitService
               .Setup(c => c.SendMessage(It.IsAny<MessageQueueType>(), It.IsAny<MessageDto>()));
            MockUnitOfWork
                .Setup(c => c.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(new User()));

            var command = new RegisterUserCommand(string.Empty, string.Empty, string.Empty);
            var commandHandler = new RegisterUserCommandHandler(MockUnitOfWork.Object, MockRabbitService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<ValidationException>(HandleCommand);
            Assert.Equal("Email address is busy.", exception.Message);
        }

        [Fact]
        public async Task RegisterUser_ForAvaliableEmail_ShouldAddUserAsync()
        {
            // Arrange
            MockRabbitService
               .Setup(c => c.SendMessage(It.IsAny<MessageQueueType>(), It.IsAny<MessageDto>()));

        }
    }
}
