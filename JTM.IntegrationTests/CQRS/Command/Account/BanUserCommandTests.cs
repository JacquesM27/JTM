using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.Exceptions;
using Moq;

namespace JTM.IntegrationTests.CQRS.Command.Account
{
    public class BanUserCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task BanUser_ForNotExistingUser_ShouldThrowsAuthExceptionWithUserNotFoundMessageAsync()
        {
            // Arrange
            _mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(null));

            var command = new BanUserCommand(0);
            var commandHandler = new BanUserCommandHandler(_mockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        [Fact]
        public async Task BanUser_ForValidUser_ShouldBanUserAsync()
        {
            // Arrange
            User tmpUser = new()
            {
                Id = 1,
                Banned = false
            };
            _mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new BanUserCommand(tmpUser.Id);
            var commandHandler = new BanUserCommandHandler(_mockUnitOfWork.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(tmpUser.Banned);
        }

        [Fact]
        public async Task BanUser_ForBannedUser_ShouldKeepBanUserAsync()
        {
            // Arrange
            User tmpUser = new()
            {
                Id = 1,
                Banned = true
            };
            _mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new BanUserCommand(tmpUser.Id);
            var commandHandler = new BanUserCommandHandler(_mockUnitOfWork.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(tmpUser.Banned);
        }
    }
}
