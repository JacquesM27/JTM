using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.DTO.Account.RegisterUser;
using JTM.Exceptions;
using Moq;
using System.Linq.Expressions;

namespace JTM.UnitTests.CQRS_Tests.Command.Account
{
    public class RefreshConfirmTokenCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task RefreshConfirmToken_ForNotExistingUser_ShouldThrowsAuthExceptionWithInvalidUserMessage()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(null));

            var command = new RefreshConfirmTokenCommand(string.Empty);
            var commandHandler = new RefreshConfirmTokenCommandHandler(MockUnitOfWork.Object, MockRabbitService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        [Fact]
        public async Task RefreshConfirmToken_ForConfirmedUser_ShouldThrowsAuthExceptionWithInvalidUserMessage()
        {
            // Arrange
            var confirmedUser = new User()
            {
                EmailConfirmed = true,
            };

            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(confirmedUser));

            var command = new RefreshConfirmTokenCommand(string.Empty);
            var commandHandler = new RefreshConfirmTokenCommandHandler(MockUnitOfWork.Object, MockRabbitService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("User already confirmed.", exception.Message);
        }

        [Fact]
        public async Task RefreshConfirmToken_ForValidUser_ShouldPassWithoutException()
        {
            // Arrange
            var confirmedUser = new User()
            {
                EmailConfirmed = false,
            };

            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(confirmedUser));
            MockUnitOfWork
                .Setup(x => x.SaveChangesAsync());
            MockRabbitService
                .Setup(c => c.SendMessage(It.IsAny<Enum.MessageQueueType>(), It.IsAny<MessageDto>()));

            var command = new RefreshConfirmTokenCommand(string.Empty);
            var commandHandler = new RefreshConfirmTokenCommandHandler(MockUnitOfWork.Object, MockRabbitService.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
        }
    }
}
