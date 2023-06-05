﻿using JTM.CQRS.Command.Account;
using JTM.Data;
using JTM.Exceptions;
using JTM.Model;
using Microsoft.EntityFrameworkCore;

namespace JTM.IntegrationTests.CQRS.Command.Account
{
    public class BanUserCommandTests
    {
        private readonly DataContext _dataContext;

        public BanUserCommandTests()
        {
            DbContextOptions<DataContext> contextOptions = new DbContextOptionsBuilder<DataContext>()
               .UseInMemoryDatabase(databaseName: "InMemory_JTM")
               .Options;
            _dataContext = new DataContext(contextOptions);
        }

        [Fact]
        public async Task BanUser_ForNotExistingUser_ShouldThrowsAuthExceptionAsync()
        {
            // Arrange
            var command = new BanUserCommand(0);
            var commandHandler = new BanUserCommandHandler(_dataContext);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
        }

        [Fact]
        public async Task BanUser_ForValidUser_ShouldBanUserAsync()
        {
            // Arrange
            var tmpUser = await _dataContext.Users.AddAsync(new User() { Email = "test", Banned = false });
            await _dataContext.SaveChangesAsync();
            var command = new BanUserCommand(tmpUser.Entity.Id);
            var commandHandler = new BanUserCommandHandler(_dataContext);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(tmpUser.Entity.Banned);
        }
    }
}
