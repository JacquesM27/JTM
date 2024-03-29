﻿using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.Exceptions;
using Moq;

namespace JTM.UnitTests.CQRS_Tests.Command.Account
{
    public class ConfirmAccountCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task ConfirmAccount_ForNotExistingUser_ShouldThrowAuthExceptionWithInvalidUserMessage()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(null));

            var command = new ConfirmAccountCommand(It.IsAny<int>(), string.Empty);
            var commandHandler = new ConfirmAccountCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        [Fact]
        public async Task ConfirmAccount_ForAlreadyConfirmedUser_ShouldThrowAuthExceptionWithUserConfirmedMessage()
        {
            // Arrange
            User tmpUser = new()
            {
                Id = 1,
                EmailConfirmed = true
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new ConfirmAccountCommand(tmpUser.Id, string.Empty);
            var commandHandler = new ConfirmAccountCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("User already confirmed.", exception.Message);
        }

        [Fact]
        public async Task ConfirmAccount_ForTokenExpired_ShouldThrowAuthExceptionWithTokenExpiredMessage()
        {
            // Arrange
            User tmpUser = new()
            {
                Id = 1,
                EmailConfirmed = false,
                ActivationTokenExpires = DateTime.UtcNow.AddHours(-1),
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new ConfirmAccountCommand(tmpUser.Id, string.Empty);
            var commandHandler = new ConfirmAccountCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Token expired.", exception.Message);
        }

        [Fact]
        public async Task ConfirmAccount_ForInvalidToken_ShouldThrowAuthExceptionWithInvalidTokenMessage()
        {
            // Arrange
            string tmpToken = Guid.NewGuid().ToString();
            User tmpUser = new()
            {
                Id = 1,
                EmailConfirmed = false,
                ActivationTokenExpires = DateTime.UtcNow.AddHours(1),
                ActivationToken = tmpToken
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new ConfirmAccountCommand(tmpUser.Id, string.Empty);
            var commandHandler = new ConfirmAccountCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid token.", exception.Message);
        }

        [Fact]
        public async Task ConfirmAccount_ForValidUser_ShouldActivateAccount()
        {
            // Arrange
            string tmpToken = Guid.NewGuid().ToString();
            User tmpUser = new()
            {
                Id = 1,
                EmailConfirmed = false,
                ActivationTokenExpires = DateTime.UtcNow.AddHours(1),
                ActivationToken = tmpToken
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new ConfirmAccountCommand(tmpUser.Id, tmpToken);
            var commandHandler = new ConfirmAccountCommandHandler(MockUnitOfWork.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(tmpUser.EmailConfirmed);
        }
    }
}
