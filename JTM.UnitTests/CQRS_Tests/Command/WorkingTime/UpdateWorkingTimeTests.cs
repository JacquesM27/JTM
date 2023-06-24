using JTM.CQRS.Command.WorkingTime;
using JTM.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTM.UnitTests.CQRS_Tests.Command.WorkingTime
{
    public class UpdateWorkingTimeTests : UnitTestBase
    {
        [Fact]
        public async Task UpdateWorkingTime_ForDiffrentIds_ShouldThrowWorkingTImeException()
        {
            // Arrange
            int tmpHeaderId = 1;
            int tmpRouteId = 2;
            var command = new UpdateWorkingTimeCommand(
                headerId: tmpHeaderId,
                routeId: tmpRouteId,
                workingDate: It.IsAny<DateTime>(),
                secondsOfWork: It.IsAny<int>(),
                note: It.IsAny<string>(),
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
    }
}
