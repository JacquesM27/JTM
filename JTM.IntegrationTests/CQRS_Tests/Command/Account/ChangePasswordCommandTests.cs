using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.Exceptions;
using JTM.Helper.PasswordHelper;
using Moq;
using System.Data.SqlTypes;

namespace JTM.IntegrationTests.CQRS_Tests.Command.Account
{
    public class ChangePasswordCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task ChangePassword_ForNotExistingUser_ShouldThrowAuthExceptionWithMessageInvalidUser()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(null));

            var command = new ChangePasswordCommand(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>());
            var commandHandler = new ChangePasswordCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        [Fact]
        public async Task ChangePassword_ForExpiredToken_ShouldThrowAuthExceptionWithMessageTokenExpires()
        {
            // Arrange
            User tmpUser = new()
            {
                Id = 1,
                PasswordTokenExpires = (DateTime)SqlDateTime.MinValue
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new ChangePasswordCommand(tmpUser.Id, It.IsAny<string>(), It.IsAny<string>());
            var commandHandler = new ChangePasswordCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Token expires.", exception.Message);
        }

        [Fact]
        public async Task ChangePassword_ForInvalidToken_ShouldThrowAuthExceptionWithMessageInvalidToken()
        {
            // Arrange
            User tmpUser = new()
            {
                Id = 1,
                PasswordTokenExpires = DateTime.UtcNow.AddHours(1),
                PasswordResetToken = Guid.NewGuid().ToString()
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new ChangePasswordCommand(tmpUser.Id, It.IsAny<string>(), Guid.NewGuid().ToString());
            var commandHandler = new ChangePasswordCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid token.", exception.Message);
        }

        [Fact]
        public async Task ChangePassword_ForValidData_ShouldResetPassword()
        {
            // Arrange
            string tmpToken = Guid.NewGuid().ToString();
            string password = Guid.NewGuid().ToString();
            User tmpUser = new()
            {
                Id = 1,
                PasswordTokenExpires = DateTime.UtcNow.AddHours(1),
                PasswordResetToken = tmpToken
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(tmpUser));

            var command = new ChangePasswordCommand(tmpUser.Id, password, tmpToken);
            var commandHandler = new ChangePasswordCommandHandler(MockUnitOfWork.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(PasswordHelper.VerifyPasswordHash(password, tmpUser.PasswordHash, tmpUser.PasswordSalt));
        }
    }
}
