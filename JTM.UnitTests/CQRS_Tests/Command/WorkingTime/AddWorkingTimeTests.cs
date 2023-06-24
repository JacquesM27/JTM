using JTM.CQRS.Command.WorkingTime;
using JTM.Exceptions;
using Moq;
using Model = JTM.Data.Model;

namespace JTM.UnitTests.CQRS_Tests.Command.WorkingTime
{
    public class AddWorkingTimeTests : UnitTestBase
    {
        [Fact]
        public async Task AddWorkingTime_ForInvalidUser_ShouldThrowAuthException()
        {
            // Arrange
            int tmpUserId = 1;
            MockUnitOfWork
                .Setup(c => c.UserRepository.AnyAsync(tmpUserId))
                .Returns(Task.FromResult(false));

            var command = new AddWorkingTimeCommand(
                workingDate: It.IsAny<DateTime>(),
                secondsOfWork: It.IsAny<int>(),
                note: string.Empty,
                companyId: It.IsAny<int>(),
                employeeId: tmpUserId,
                authorId: tmpUserId);
            var commandHandler = new AddWorkingTimeCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<AuthException>(HandleCommand);
            Assert.Equal($"User with id:{tmpUserId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task AddWorkingTime_ForInvalidCompany_ShouldThrowCompanyException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.AnyAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            int tmpCompanyId = 1;
            MockUnitOfWork
                .Setup(x => x.CompanyRepository.AnyAsync(tmpCompanyId))
                .Returns(Task.FromResult(false));

            var command = new AddWorkingTimeCommand(
                workingDate: It.IsAny<DateTime>(),
                secondsOfWork: It.IsAny<int>(),
                note: string.Empty,
                companyId: tmpCompanyId,
                employeeId: It.IsAny<int>(),
                authorId: It.IsAny<int>());
            var commandHandler = new AddWorkingTimeCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<CompanyException>(HandleCommand);
            Assert.Equal($"Company with id:{tmpCompanyId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task AddWorkingTime_ForValidModelWithoutCompany_ShouldPassWithoutException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.AnyAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            MockUnitOfWork
                .Setup(x => x.WorkingTimeRepository.AddAsync(It.IsAny<Model.WorkingTime>()));

            // Act
            var command = new AddWorkingTimeCommand(
                workingDate: It.IsAny<DateTime>(),
                secondsOfWork: It.IsAny<int>(),
                note: string.Empty,
                companyId: null,
                employeeId: It.IsAny<int>(),
                authorId: It.IsAny<int>());
            var commandHandler = new AddWorkingTimeCommandHandler(MockUnitOfWork.Object);
            await commandHandler.Handle(command, default);

            // Assert
        }

        [Fact]
        public async Task AddWorkingTime_ForValidModel_ShouldPassWithoutException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.UserRepository.AnyAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            MockUnitOfWork
                .Setup(x => x.CompanyRepository.AnyAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            MockUnitOfWork
                .Setup(x => x.WorkingTimeRepository.AddAsync(It.IsAny<Model.WorkingTime>()));

            // Act
            var command = new AddWorkingTimeCommand(
                workingDate: It.IsAny<DateTime>(),
                secondsOfWork: It.IsAny<int>(),
                note: string.Empty,
                companyId: It.IsAny<int>(),
                employeeId: It.IsAny<int>(),
                authorId: It.IsAny<int>());
            var commandHandler = new AddWorkingTimeCommandHandler(MockUnitOfWork.Object);
            await commandHandler.Handle(command, default);

            // Assert
        }
    }
}
