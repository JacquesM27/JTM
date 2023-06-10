using JTM.CQRS.Command.Account;
using JTM.Data;
using JTM.Exceptions;
using JTM.Data.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using System.Data.SqlTypes;

namespace JTM.IntegrationTests.CQRS.Command.Account
{
    public class ConfirmAccountCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task ConfirmAccount_ForNotExistingUser_ShouldThrowAuthExceptionWithInvalidUserMessageAsync()
        {
            // Arrange
            _mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(null));

            var command = new ConfirmAccountCommand(It.IsAny<int>(), It.IsAny<string>());
            var commandHandler = new ConfirmAccountCommandHandler(_mockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        [Fact]
        public async Task ConfirmAccount_ForAlreadyConfirmedUser_ShouldThrowAuthExceptionWithUserConfirmedMessageAsync()
        {
            // Arrange
            User tmpUser = new()
            {
                Id = 1,
                EmailConfirmed = true
            };
            _mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new ConfirmAccountCommand(tmpUser.Id, It.IsAny<string>());
            var commandHandler = new ConfirmAccountCommandHandler(_mockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("User already confirmed.", exception.Message);
        }

        [Fact]
        public async Task ConfirmAccount_ForTokenExpired_ShouldThrowAuthExceptionWithTokenExpiredMessageAsync()
        {
            // Arrange
            User tmpUser = new()
            {
                Id = 1,
                EmailConfirmed = false,
                ActivationTokenExpires = DateTime.Now.AddHours(-1),
            };
            _mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new ConfirmAccountCommand(tmpUser.Id, It.IsAny<string>());
            var commandHandler = new ConfirmAccountCommandHandler(_mockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Token expired.", exception.Message);
        }

        [Fact]
        public async Task ConfirmAccount_ForInvalidToken_ShouldThrowAuthExceptionWithInvalidTokenMessageAsync()
        {
            // Arrange
            string tmpToken = Guid.NewGuid().ToString();
            User tmpUser = new()
            {
                Id = 1,
                EmailConfirmed = false,
                ActivationTokenExpires = DateTime.Now.AddHours(1),
                ActivationToken = tmpToken
            };
            _mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new ConfirmAccountCommand(tmpUser.Id, string.Empty);
            var commandHandler = new ConfirmAccountCommandHandler(_mockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid token.", exception.Message);
        }

        [Fact]
        public async Task ConfirmAccount_ForValidUser_ShouldActivateAccountAsync()
        {
            // Arrange
            string tmpToken = Guid.NewGuid().ToString();
            User tmpUser = new()
            {
                Id = 1,
                EmailConfirmed = false,
                ActivationTokenExpires = DateTime.Now.AddHours(1),
                ActivationToken = tmpToken
            };
            _mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new ConfirmAccountCommand(tmpUser.Id, tmpToken);
            var commandHandler = new ConfirmAccountCommandHandler(_mockUnitOfWork.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(tmpUser.EmailConfirmed);
        }
    }
}
