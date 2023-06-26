using JTM.CQRS.Command.WorkingTime;
using JTM.Exceptions;
using Moq;
using Model = JTM.Data.Model;

namespace JTM.UnitTests.CQRS_Tests.Command.WorkingTime
{
    public class UpdateWorkingTimeTests : UnitTestBase
    {
        [Fact]
        public async Task UpdateWorkingTime_ForDiffrentIds_ShouldThrowWorkingTimeException()
        {
            // Arrange
            int tmpHeaderId = 1;
            int tmpRouteId = 2;
            var command = new UpdateWorkingTimeCommand(
                headerId: tmpHeaderId,
                routeId: tmpRouteId,
                workingDate: It.IsAny<DateTime>(),
                secondsOfWork: It.IsAny<int>(),
                note: string.Empty,
                companyId: It.IsAny<int>(),
                employeeId: It.IsAny<int>(),
                editorId: It.IsAny<int>()
                );
            var commandHandler = new UpdateWorkingTimeCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAsync<WorkingTimeException>(HandleCommand);
            Assert.Equal($"Id from header({tmpHeaderId}) does not equal id from route({tmpRouteId}).", exception.Message);
        }

        [Fact]
        public async Task UpdateWorkingTime_ForNotExistWorkingTime_ShouldThrowWorkingTimeException()
        {
            // Arrange
            int tmpId = 1;
            MockUnitOfWork
                .Setup(x => x.WorkingTimeRepository.GetByIdAsync(tmpId))
                .Returns(Task.FromResult<Model.WorkingTime?>(null));

            var command = new UpdateWorkingTimeCommand(
               headerId: tmpId,
               routeId: tmpId,
               workingDate: It.IsAny<DateTime>(),
               secondsOfWork: It.IsAny<int>(),
               note: string.Empty,
               companyId: It.IsAny<int>(),
               employeeId: It.IsAny<int>(),
               editorId: It.IsAny<int>()
               );
            var commandHandler = new UpdateWorkingTimeCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAsync<WorkingTimeException>(HandleCommand);
            Assert.Equal($"Working time with id: {tmpId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateWorkingTime_ForNotExistWorkingEmployee_ShouldThrowWorkingTimeException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.WorkingTimeRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Model.WorkingTime?>(new Model.WorkingTime()));
            int tmpEmployeeId = 1;
            MockUnitOfWork
                .Setup(x => x.UserRepository.AnyAsync(tmpEmployeeId))
                .Returns(Task.FromResult(false));

            var command = new UpdateWorkingTimeCommand(
              headerId: It.IsAny<int>(),
              routeId: It.IsAny<int>(),
              workingDate: It.IsAny<DateTime>(),
              secondsOfWork: It.IsAny<int>(),
              note: string.Empty,
              companyId: It.IsAny<int>(),
              employeeId: tmpEmployeeId,
              editorId: It.IsAny<int>()
              );
            var commandHandler = new UpdateWorkingTimeCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAsync<WorkingTimeException>(HandleCommand);
            Assert.Equal($"User with id:{tmpEmployeeId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateWorkingTime_ForNotExistWorkingAuthor_ShouldThrowWorkingTimeException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.WorkingTimeRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Model.WorkingTime?>(new Model.WorkingTime()));
            int tmpEmployeeId = 2;
            MockUnitOfWork
               .Setup(x => x.UserRepository.AnyAsync(tmpEmployeeId))
               .Returns(Task.FromResult(true));
            int tmpEditorId = 1;
            MockUnitOfWork
                .Setup(x => x.UserRepository.AnyAsync(tmpEditorId))
                .Returns(Task.FromResult(false));

            var command = new UpdateWorkingTimeCommand(
              headerId: It.IsAny<int>(),
              routeId: It.IsAny<int>(),
              workingDate: It.IsAny<DateTime>(),
              secondsOfWork: It.IsAny<int>(),
              note: string.Empty,
              companyId: It.IsAny<int>(),
              employeeId: tmpEmployeeId,
              editorId: tmpEditorId
              );
            var commandHandler = new UpdateWorkingTimeCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAsync<WorkingTimeException>(HandleCommand);
            Assert.Equal($"User with id:{tmpEditorId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateWorkingTime_ForNotExistCompany_ShouldThrowWorkingTimeException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.WorkingTimeRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Model.WorkingTime?>(new Model.WorkingTime()));
            MockUnitOfWork
               .Setup(x => x.UserRepository.AnyAsync(It.IsAny<int>()))
               .Returns(Task.FromResult(true));
            int tmpCompanyId = 1;
            MockUnitOfWork
                .Setup(x => x.CompanyRepository.AnyAsync(tmpCompanyId))
                .Returns(Task.FromResult(false));

            var command = new UpdateWorkingTimeCommand(
              headerId: It.IsAny<int>(),
              routeId: It.IsAny<int>(),
              workingDate: It.IsAny<DateTime>(),
              secondsOfWork: It.IsAny<int>(),
              note: string.Empty,
              companyId: tmpCompanyId,
              employeeId: It.IsAny<int>(),
              editorId: It.IsAny<int>()
              );
            var commandHandler = new UpdateWorkingTimeCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAsync<WorkingTimeException>(HandleCommand);
            Assert.Equal($"Company with id:{tmpCompanyId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateWorkingTime_ForValidModelWithCompany_ShouldPassWithoutException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.WorkingTimeRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Model.WorkingTime?>(new Model.WorkingTime()));
            MockUnitOfWork
               .Setup(x => x.UserRepository.AnyAsync(It.IsAny<int>()))
               .Returns(Task.FromResult(true));
            MockUnitOfWork
                .Setup(x => x.CompanyRepository.AnyAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            MockUnitOfWork
                .Setup(x => x.SaveChangesAsync());

            var command = new UpdateWorkingTimeCommand(
              headerId: It.IsAny<int>(),
              routeId: It.IsAny<int>(),
              workingDate: It.IsAny<DateTime>(),
              secondsOfWork: It.IsAny<int>(),
              note: string.Empty,
              companyId: It.IsAny<int>(),
              employeeId: It.IsAny<int>(),
              editorId: It.IsAny<int>()
              );
            var commandHandler = new UpdateWorkingTimeCommandHandler(MockUnitOfWork.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
        }

        [Fact]
        public async Task UpdateWorkingTime_ForValidModelWithoutCompany_ShouldPassWithoutException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(x => x.WorkingTimeRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Model.WorkingTime?>(new Model.WorkingTime()));
            MockUnitOfWork
               .Setup(x => x.UserRepository.AnyAsync(It.IsAny<int>()))
               .Returns(Task.FromResult(true));
            MockUnitOfWork
                .Setup(x => x.SaveChangesAsync());

            var command = new UpdateWorkingTimeCommand(
              headerId: It.IsAny<int>(),
              routeId: It.IsAny<int>(),
              workingDate: It.IsAny<DateTime>(),
              secondsOfWork: It.IsAny<int>(),
              note: string.Empty,
              companyId: null,
              employeeId: It.IsAny<int>(),
              editorId: It.IsAny<int>()
              );
            var commandHandler = new UpdateWorkingTimeCommandHandler(MockUnitOfWork.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
        }
    }
}
