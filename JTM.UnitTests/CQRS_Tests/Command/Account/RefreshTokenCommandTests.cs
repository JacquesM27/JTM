using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.DTO.Account;
using JTM.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JTM.UnitTests.CQRS_Tests.Command.Account
{
    public class RefreshTokenCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task RefreshToken_ForMissingToken_ShouldThrowsAuthException()
        {
            // Arrange
            var command = new RefreshTokenCommand(string.Empty);
            var commandHandler = new RefreshTokenCommandHandler(MockUnitOfWork.Object, MockTokenService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Missing token.", exception.Message);
        }

        [Fact]
        public async Task RefreshToken_ForNotFoundUser_ShouldThrowsAuthException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(null));

            var command = new RefreshTokenCommand("x");
            var commandHandler = new RefreshTokenCommandHandler(MockUnitOfWork.Object, MockTokenService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid token.", exception.Message);
        }

        [Fact]
        public async Task RefreshToken_ForUserBanned_ShouldThrowsAuthException()
        {
            // Arrange
            var bannedUser = new User()
            {
                Banned = true
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(bannedUser));

            var command = new RefreshTokenCommand("x");
            var commandHandler = new RefreshTokenCommandHandler(MockUnitOfWork.Object, MockTokenService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Account banned.", exception.Message);
        }

        [Fact]
        public async Task RefreshToken_ForExpiredToken_ShouldThrowsAuthException()
        {
            // Arrange
            var bannedUser = new User()
            {
                Banned = false,
                TokenExpires = DateTime.UtcNow.AddMinutes(-1)
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(bannedUser));

            var command = new RefreshTokenCommand("x");
            var commandHandler = new RefreshTokenCommandHandler(MockUnitOfWork.Object, MockTokenService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Token expired.", exception.Message);
        }

        [Fact]
        public async Task RefreshToken_ForValidData_ShouldThrowsAuthException()
        {
            // Arrange
            var bannedUser = new User()
            {
                Banned = false,
                TokenExpires = DateTime.UtcNow.AddMinutes(1)
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(bannedUser));
            MockTokenService
                .Setup(x => x.CreateToken(bannedUser))
                .Returns(string.Empty);
            MockTokenService
                .Setup(x => x.CreateRefreshToken())
                .Returns(new RefreshTokenDto());
            MockUnitOfWork
                .Setup(x => x.SaveChangesAsync());

            var command = new RefreshTokenCommand("x");
            var commandHandler = new RefreshTokenCommandHandler(MockUnitOfWork.Object, MockTokenService.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
        }
    }
}
