using JTM.CQRS.Command.Account;
using JTM.Data.Model;
using JTM.DTO.Account.RegisterUser;
using JTM.Enum;
using JTM.Exceptions;
using Moq;
using System.Data.SqlTypes;
using System.Linq.Expressions;

namespace JTM.IntegrationTests.CQRS_Tests.Command.Account
{
    public class ForgetPasswordCommandTests : AccountTestsBase
    {
        [Fact]
        public async Task ForgetPassword_ForNotExitsingUser_ShouldThrowsAuthExeptionWithUserNotFoundMessageAsync()
        {
            // Arrange
            MockRabbitService
                .Setup(c => c.SendMessage(It.IsAny<MessageQueueType>(), It.IsAny<MessageDto>()));
            MockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User?>(null));

            var command = new ForgetPasswordCommand(string.Empty);
            var commandHandler = new ForgetPasswordUserCommandHandler(MockUnitOfWork.Object, MockRabbitService.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal("Invalid user.", exception.Message);
        }

        [Fact]
        public async Task ForgetPassword_ForExistingUser_ShouldSetPasswordResetTokenAndDateAsync()
        {
            // Arrange
            string email = "test@test.com";
            var tmpUser = new User()
            {
                Id = 1,
                Email = email,
                PasswordTokenExpires = (DateTime)SqlDateTime.MinValue,
                PasswordResetToken = null
            };
            MockUnitOfWork
                .Setup(x => x.UserRepository.QuerySingleAsync(It.IsAny<Expression<Func<User,bool>>>()))
                .Returns(Task.FromResult(tmpUser));
            MockRabbitService
               .Setup(c => c.SendMessage(It.IsAny<MessageQueueType>(), It.IsAny<MessageDto>()));

            var command = new ForgetPasswordCommand(email);
            var commandHandler = new ForgetPasswordUserCommandHandler(MockUnitOfWork.Object, MockRabbitService.Object);

            // Act 
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(tmpUser.PasswordTokenExpires > DateTime.UtcNow.AddHours(23));
            Assert.True(Convert.FromBase64String(tmpUser.PasswordResetToken).Length == 64);
        }
    }
}
