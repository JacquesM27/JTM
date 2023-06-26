using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.DTO.Account;
using JTM.Exceptions;
using JTM.Helper.PasswordHelper;
using Moq;
using System.Linq.Expressions;

namespace JTM.UnitTests.CQRS_Tests.Command.Account
{
    public class LoginUserCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task LoginUser_ForNotExistingUser_ShouldThrowAuthExceptionWithMessageInvalidUser()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(null));

            var command = new LoginCommand(string.Empty, string.Empty);
            var commandHandler = new LoginCommandHandler(MockUnitOfWork.Object, MockTokenService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        [Fact]
        public async Task LoginUser_ForBlockedAccount_ShouldThrowAuthException()
        {
            // Arrange
            var bannedUser = new User()
            {
                Banned = true
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(bannedUser));

            var command = new LoginCommand(string.Empty, string.Empty);
            var commandHandler = new LoginCommandHandler(MockUnitOfWork.Object, MockTokenService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Account blocked.", exception.Message);
        }

        [Fact]
        public async Task LoginUser_ForNotConfirmedEmail_ShouldThrowAuthException()
        {
            // Arrange
            var notConfirmedUser = new User()
            {
                Banned = false,
                EmailConfirmed = false
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(notConfirmedUser));

            var command = new LoginCommand(string.Empty, string.Empty);
            var commandHandler = new LoginCommandHandler(MockUnitOfWork.Object, MockTokenService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Account not activated.", exception.Message);
        }

        [Fact]
        public async Task LoginUser_ForWrongPassword_ShouldThrowAuthException()
        {
            // Arrange
            PasswordHelper.CreatePasswordHash("This!Is123Very()Hard*#Passowrd_2222&Flowrsx", out byte[] passwordHash, out byte[] passwordSalt);
            var someUser = new User()
            {
                Banned = false,
                EmailConfirmed = true,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(someUser));

            var command = new LoginCommand(string.Empty, string.Empty);
            var commandHandler = new LoginCommandHandler(MockUnitOfWork.Object, MockTokenService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Wrong password.", exception.Message);
        }

        [Fact]
        public async Task LoginUser_ForValidData_ShouldPassWithoutException()
        {
            // Arrange
            string tmpUserPassword = "This!Is123Very()Hard*#Passowrd_2222&Flowrsx";
            PasswordHelper.CreatePasswordHash(tmpUserPassword, out byte[] passwordHash, out byte[] passwordSalt);
            var someUser = new User()
            {
                Banned = false,
                EmailConfirmed = true,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult<User?>(someUser));
            MockTokenService
                .Setup(x => x.CreateToken(someUser))
                .Returns(string.Empty);
            MockTokenService
                .Setup(x => x.CreateRefreshToken())
                .Returns(new RefreshTokenDto());
            MockUnitOfWork
                .Setup(x => x.SaveChangesAsync());

            var command = new LoginCommand(string.Empty, tmpUserPassword);
            var commandHandler = new LoginCommandHandler(MockUnitOfWork.Object, MockTokenService.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
        }
    }
}
