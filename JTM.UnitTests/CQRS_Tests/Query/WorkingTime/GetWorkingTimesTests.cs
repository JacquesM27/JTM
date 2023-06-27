using JTM.CQRS.Query.WorkingTime;
using JTM.DTO.WorkingTime;
using Moq;
using System.Linq.Expressions;
using Model = JTM.Data.Model;

namespace JTM.UnitTests.CQRS_Tests.Query.WorkingTime
{
    public class GetWorkingTimesTests : UnitTestBase
    {
        [Fact]
        public async Task GetWorkingTimes_ForAnyData_ShouldReturnEmptyCollection()
        {
            // Arrange
            MockUnitOfWork
                .Setup(c => c.WorkingTimeRepository.QueryAsync(
                        It.IsAny<Expression<Func<Model.WorkingTime, bool>>>(),
                        It.IsAny<Expression<Func<Model.WorkingTime, object>>[]>()))
                .Returns(Task.FromResult(Enumerable.Empty<Model.WorkingTime>()));

            var command = new GetWorkingTimesQuery(
                It.IsAny<IEnumerable<int>>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<IEnumerable<int>>(),
                string.Empty,
                It.IsAny<int>(),
                It.IsAny<int>());
            var commandHandler = new GetWorkingTimesQueryHandler(MockUnitOfWork.Object);

            // Act
            var result = await commandHandler.Handle(command, default);

            // Assert
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<BasicWorkingTimeDto>>(result);
        }
    }
}
