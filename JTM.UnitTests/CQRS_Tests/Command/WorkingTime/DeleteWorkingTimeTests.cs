using JTM.CQRS.Command.WorkingTime;
using JTM.Exceptions;
using Moq;
using Model = JTM.Data.Model;

namespace JTM.UnitTests.CQRS_Tests.Command.WorkingTime
{
    public class DeleteWorkingTimeTests : UnitTestBase
    {
        [Fact]
        public async Task DeleteWorkingTime_ForInvalidDeletor_ShouldThrowAuthException()
        {
            // Arrange
            int tmpUserId = 1;
            MockUnitOfWork
                .Setup(c => c.UserRepository.AnyAsync(tmpUserId))
                .Returns(Task.FromResult(false));

            var command = new DeleteTimeCommand(It.IsAny<int>(), tmpUserId);
            var commandHandler = new DeleteTimeCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal($"User with id:{tmpUserId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task DeleteWorkingTime_ForInvalidWorkingTimeId_ShouldThrowWorkingTimeException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.AnyAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            int tmpWtId = 1;
            MockUnitOfWork
                .Setup(c => c.WorkingTimeRepository.GetByIdAsync(tmpWtId))
                .Returns(Task.FromResult<Model.WorkingTime?>(null));

            var command = new DeleteTimeCommand(tmpWtId, It.IsAny<int>());
            var commandHandler = new DeleteTimeCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<WorkingTimeException>(HandleCommand);
            Assert.Equal($"Working time with id:{tmpWtId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task DeleteWorkingTime_ForValidData_ShouldPassWithoutException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.AnyAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            MockUnitOfWork
                .Setup(c => c.WorkingTimeRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Model.WorkingTime?>(new Model.WorkingTime()));
            MockUnitOfWork
                .Setup(c => c.WorkingTimeRepository.RemoveAsync(It.IsAny<Model.WorkingTime>()));

            var command = new DeleteTimeCommand(It.IsAny<int>(), It.IsAny<int>());  
            var commandHandler = new DeleteTimeCommandHandler(MockUnitOfWork.Object); 
            
            // Act
            await commandHandler.Handle(command, default);

            // Assert
        }
    }
}
