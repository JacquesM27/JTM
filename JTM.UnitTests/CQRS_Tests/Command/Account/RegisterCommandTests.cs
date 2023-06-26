using FluentValidation;
using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.DTO.Account.RegisterUser;
using JTM.Enum;
using Moq;
using System.Linq.Expressions;

namespace JTM.UnitTests.CQRS_Tests.Command.Account
{
    public class RegisterCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task RegisterUser_ForBusyEmail_ShouldThrowValidationExceptionWithBusyMessage()
        {
            // Arrange
            MockRabbitService
               .Setup(c => c.SendMessage(It.IsAny<MessageQueueType>(), It.IsAny<MessageDto>()));
            MockUnitOfWork
                .Setup(c => c.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(new User()));

            var command = new RegisterCommand(string.Empty, string.Empty, string.Empty, UserRole.user);
            var commandHandler = new RegisterCommandHandler(MockUnitOfWork.Object, MockRabbitService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<ValidationException>(HandleCommand);
            Assert.Equal("Email address is busy.", exception.Message);
        }

        [Fact]
        public async Task RegisterUser_ForAvaliableEmail_ShouldAddUser()
        {
            // Arrange
            MockRabbitService
               .Setup(c => c.SendMessage(It.IsAny<MessageQueueType>(), It.IsAny<MessageDto>()));
            MockUnitOfWork
                .Setup(c => c.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(null));
            MockUnitOfWork
                .Setup(c => c.UserRepository.AddAsync(It.IsAny<User>()));

            var command = new RegisterCommand(string.Empty, string.Empty, string.Empty, UserRole.user);
            var commandHandler = new RegisterCommandHandler(MockUnitOfWork.Object, MockRabbitService.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
        }
    }
}
