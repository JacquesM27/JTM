using JTM.CQRS.Command.Account;
using JTM.Data;
using JTM.Exceptions;
using JTM.Helper.PasswordHelper;
using JTM.Data.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data.SqlTypes;

namespace JTM.IntegrationTests.CQRS.Command.Account
{
    public class ChangePasswordCommandTests
    {
        private readonly DataContext _dataContext;

        public ChangePasswordCommandTests()
        {
            DbContextOptions<DataContext> contextOptions = new DbContextOptionsBuilder<DataContext>()
               .UseInMemoryDatabase(databaseName: "InMemory_JTM")
               .Options;
            _dataContext = new DataContext(contextOptions);
        }

        [Fact]
        public async Task ChangePassword_ForNotExistingUser_ShouldThrowAuthExceptionWithMessageInvalidUserAsync()
        {
            // Arrange
            var command = new ChangePasswordCommand(It.IsAny<int>(),It.IsAny<string>(),It.IsAny<string>());
            var commandHandler = new ChangePasswordCommandHandler(_dataContext);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        [Fact]
        public async Task ChangePassword_ForExpiredToken_ShouldThrowAuthExceptionWithMessageTokenExpiresAsync()
        {
            // Arrange
            var tmpUser = await _dataContext.Users.AddAsync(new User()
            {
                PasswordTokenExpires = (DateTime)SqlDateTime.MinValue
            });
            await _dataContext.SaveChangesAsync();
            var command = new ChangePasswordCommand(tmpUser.Entity.Id, It.IsAny<string>(), It.IsAny<string>());
            var commandHandler = new ChangePasswordCommandHandler(_dataContext);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Token expires.", exception.Message);
        }

        [Fact]
        public async Task ChangePassword_ForInvalidToken_ShouldThrowAuthExceptionWithMessageInvalidTokenAsync()
        {
            // Arrange
            var tmpUser = await _dataContext.Users.AddAsync(new User()
            {
                PasswordTokenExpires = DateTime.Now.AddHours(-1),
                PasswordResetToken = Guid.NewGuid().ToString()
            });
            await _dataContext.SaveChangesAsync();
            var command = new ChangePasswordCommand(tmpUser.Entity.Id, It.IsAny<string>(), Guid.NewGuid().ToString());
            var commandHandler = new ChangePasswordCommandHandler(_dataContext);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid token.", exception.Message);
        }

        [Fact]
        public async Task ChangePassword_ForValidData_ShouldResetPasswordAsync()
        {
            // Arrange
            string tmpToken = Guid.NewGuid().ToString();
            string password = Guid.NewGuid().ToString();
            var tmpUser = await _dataContext.Users.AddAsync(new User()
            {
                PasswordTokenExpires = DateTime.Now.AddHours(1),
                PasswordResetToken = tmpToken
            });
            await _dataContext.SaveChangesAsync();
            var command = new ChangePasswordCommand(tmpUser.Entity.Id, password, tmpToken);
            var commandHandler = new ChangePasswordCommandHandler(_dataContext);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(PasswordHelper.VerifyPasswordHash(password, tmpUser.Entity.PasswordHash, tmpUser.Entity.PasswordSalt));
        }
    }
}
