using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.Exceptions;
using Moq;

namespace JTM.UnitTests.CQRS_Tests.Command.Account
{
    public class BanUserCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task BanUser_ForNotExistingUser_ShouldThrowsAuthExceptionWithUserNotFoundMessage()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(null));

            var command = new BanUserCommand(0);
            var commandHandler = new BanUserCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        [Fact]
        public async Task BanUser_ForValidUser_ShouldBanUser()
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

            var command = new BanUserCommand(tmpUser.Id);
            var commandHandler = new BanUserCommandHandler(MockUnitOfWork.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(tmpUser.Banned);
        }

        [Fact]
        public async Task BanUser_ForBannedUser_ShouldKeepBanUser()
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

            var command = new BanUserCommand(tmpUser.Id);
            var commandHandler = new BanUserCommandHandler(MockUnitOfWork.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(tmpUser.Banned);
        }
    }
}
