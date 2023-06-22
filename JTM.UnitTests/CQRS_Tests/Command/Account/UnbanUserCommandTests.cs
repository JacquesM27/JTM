using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.Exceptions;
using Moq;

namespace JTM.UnitTests.CQRS_Tests.Command.Account
{
    public class UnbanUserCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task UnbanUser_ForNotExistingUser_ShouldThrowsAuthExceptionWithInvalidUserMessage()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(null));

            var command = new UnbanUserCommand(0);
            var commandHandler = new UnbanUserCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        [Fact]
        public async Task UnbanUser_ForValidUser_ShouldUnbanUserAndReturnsSameId()
        {
            // Arrange
            User tmpUser = new()
            {
                Id = 1,
                Banned = true
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(tmpUser));

            var command = new UnbanUserCommand(tmpUser.Id);
            var commandHandler = new UnbanUserCommandHandler(MockUnitOfWork.Object);

            // Act
            var result = await commandHandler.Handle(command, default);

            // Assert
            Assert.False(tmpUser.Banned);
            Assert.Equal(tmpUser.Id, result);
        }

        [Fact]
        public async Task UnbanUser_ForBannedUser_ShouldKeepUnbanUserAndReturnsSameId()
        {
            // Arrange
            User tmpUser = new()
            {
                Id = 1,
                Banned = false
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(tmpUser));

            var command = new UnbanUserCommand(tmpUser.Id);
            var commandHandler = new UnbanUserCommandHandler(MockUnitOfWork.Object);

            // Act
            var result = await commandHandler.Handle(command, default);

            // Assert
            Assert.False(tmpUser.Banned);
            Assert.Equal(tmpUser.Id, result);
        }
    }
}
