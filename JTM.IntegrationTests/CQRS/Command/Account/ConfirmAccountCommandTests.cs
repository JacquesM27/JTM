using JTM.CQRS.Command.Account;
using JTM.Data;
using JTM.Exceptions;
using JTM.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data.SqlTypes;

namespace JTM.IntegrationTests.CQRS.Command.Account
{
    public class ConfirmAccountCommandTests
    {
        private readonly DataContext _dataContext;

        public ConfirmAccountCommandTests()
        {
            DbContextOptions<DataContext> contextOptions = new DbContextOptionsBuilder<DataContext>()
              .UseInMemoryDatabase(databaseName: "InMemory_JTM")
              .Options;
            _dataContext = new DataContext(contextOptions);
        }

        [Fact]
        public async Task ConfirmAccount_ForNotExistingUser_ShouldThrowAuthExceptionWithInvalidUserMessageAsync()
        {
            // Arrange
            var command = new ConfirmAccountCommand(It.IsAny<int>(), It.IsAny<string>());
            var commandHandler = new ConfirmAccountCommandHandler(_dataContext);

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
            var tmpUser = await _dataContext.Users.AddAsync(new User()
            {
                EmailConfirmed = true,
            });
            await _dataContext.SaveChangesAsync();
            var command = new ConfirmAccountCommand(tmpUser.Entity.Id, It.IsAny<string>());
            var commandHandler = new ConfirmAccountCommandHandler(_dataContext);

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
            var tmpUser = await _dataContext.Users.AddAsync(new User()
            {
                EmailConfirmed = false,
                ActivationTokenExpires = DateTime.Now.AddHours(-1),
            });
            await _dataContext.SaveChangesAsync();
            var command = new ConfirmAccountCommand(tmpUser.Entity.Id, It.IsAny<string>());
            var commandHandler = new ConfirmAccountCommandHandler(_dataContext);

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
            var tmpUser = await _dataContext.Users.AddAsync(new User()
            {
                EmailConfirmed = false,
                ActivationTokenExpires = DateTime.Now.AddHours(1),
                ActivationToken = tmpToken
            });
            await _dataContext.SaveChangesAsync();
            var command = new ConfirmAccountCommand(tmpUser.Entity.Id, "");
            var commandHandler = new ConfirmAccountCommandHandler(_dataContext);

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
            var tmpUser = await _dataContext.Users.AddAsync(new User()
            {
                EmailConfirmed = false,
                ActivationTokenExpires = DateTime.Now.AddHours(1),
                ActivationToken = tmpToken
            });
            await _dataContext.SaveChangesAsync();
            var command = new ConfirmAccountCommand(tmpUser.Entity.Id, tmpToken);
            var commandHandler = new ConfirmAccountCommandHandler(_dataContext);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(tmpUser.Entity.EmailConfirmed);
        }
    }
}
