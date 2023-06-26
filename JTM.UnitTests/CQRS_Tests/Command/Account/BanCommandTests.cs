using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.Exceptions;
using Moq;

namespace JTM.UnitTests.CQRS_Tests.Command.Account
{
    public class BanCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task BanUser_ForNotExistingUser_ShouldThrowsAuthExceptionnWithInvalidUserMessage()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(null));

            var command = new BanCommand(0);
            var commandHandler = new BanCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        [Fact]
        public async Task BanUser_ForValidUser_ShouldBanUserAndReturnsSameId()
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

            var command = new BanCommand(tmpUser.Id);
            var commandHandler = new BanCommandHandler(MockUnitOfWork.Object);

            // Act
            var result = await commandHandler.Handle(command, default);

            // Assert
            Assert.True(tmpUser.Banned);
            Assert.Equal(tmpUser.Id, result);
        }

        [Fact]
        public async Task BanUser_ForBannedUser_ShouldKeepBanUserAndReturnsSameId()
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

            var command = new BanCommand(tmpUser.Id);
            var commandHandler = new BanCommandHandler(MockUnitOfWork.Object);

            // Act
            var result = await commandHandler.Handle(command, default);

            // Assert
            Assert.True(tmpUser.Banned);
            Assert.Equal(tmpUser.Id, result);
        }
    }
}
