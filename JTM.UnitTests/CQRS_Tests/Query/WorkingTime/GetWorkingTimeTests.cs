using JTM.CQRS.Query.WorkingTime;
using JTM.DTO.WorkingTime;
using JTM.Exceptions;
using Moq;
using System.Linq.Expressions;
using Model = JTM.Data.Model;

namespace JTM.UnitTests.CQRS_Tests.Query.WorkingTime
{
    public class GetWorkingTimeTests : UnitTestBase
    {
        [Fact]
        public async Task GetWorkingTime_ForInvalidArgument_ShouldThrowsCompanyException()
        {
            // Arrange
            int tmpId = 1;
            MockUnitOfWork
                .Setup(c => c.WorkingTimeRepository.QuerySingleAsync(It.IsAny<Expression<Func<Model.WorkingTime, bool>>>()))
                .Returns(Task.FromResult<Model.WorkingTime?>(null));

            var command = new GetWorkingTimeQuery(tmpId);
            var commandHandler = new GetWorkingTimeQueryHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<WorkingTimeException>(HandleCommand);
            Assert.Equal($"Working time with id: {tmpId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task GetWorkingTime_ForExistingObject_ShouldReturnsWorkingTimeDtoObject()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            Model.WorkingTime workingTime = new()
            {
                Id = 1,
                Author = new Model.User() { Username = "AuthorName" },
                Company = new Model.Company() { Name = "Company" },
                Deleted = false,
                Employee = new Model.User() { Username = "EmployeeName" },
                LastEditor = new Model.User() { Username = "LastEditorName" },
                Note = "Note",
                SecondsOfWork = 90,
                WorkingDate = dateTime
            };
            MockUnitOfWork
                .Setup(c => c.WorkingTimeRepository.QuerySingleAsync(
                        It.IsAny<Expression<Func<Model.WorkingTime, bool>>>(),
                        It.IsAny<Expression<Func<Model.WorkingTime, object>>[]>()))
                .Returns(Task.FromResult<Model.WorkingTime?>(workingTime));

            var command = new GetWorkingTimeQuery(1);
            var commandHandler = new GetWorkingTimeQueryHandler(MockUnitOfWork.Object);

            // Act
            var result = await commandHandler.Handle(command, default);

            DetailsWorkingTimeDto detailsWtDto = new()
            {
                Id = 1,
                AuthorName = "AuthorName",
                Company = "Company",
                EmployeeName = "EmployeeName",
                LastEditorName = "LastEditorName",
                Note = "Note",
                SecondsOfWork = 90,
                WorkingDate = dateTime
            };
            Assert.Equal(detailsWtDto, result);
        }

        [Fact]
        public async Task GetWorkingTime_ForExistingObjectWithoutCompany_ShouldReturnsWorkingTimeDtoObject()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            Model.WorkingTime workingTime = new()
            {
                Id = 1,
                Author = new Model.User() { Username = "AuthorName" },
                Deleted = false,
                Employee = new Model.User() { Username = "EmployeeName" },
                LastEditor = new Model.User() { Username = "LastEditorName" },
                Note = "Note",
                SecondsOfWork = 90,
                WorkingDate = dateTime
            };
            MockUnitOfWork
                .Setup(c => c.WorkingTimeRepository.QuerySingleAsync(
                        It.IsAny<Expression<Func<Model.WorkingTime, bool>>>(),
                        It.IsAny<Expression<Func<Model.WorkingTime, object>>[]>()))
                .Returns(Task.FromResult<Model.WorkingTime?>(workingTime));

            var command = new GetWorkingTimeQuery(1);
            var commandHandler = new GetWorkingTimeQueryHandler(MockUnitOfWork.Object);

            // Act
            var result = await commandHandler.Handle(command, default);

            DetailsWorkingTimeDto detailsWtDto = new()
            {
                Id = 1,
                AuthorName = "AuthorName",
                EmployeeName = "EmployeeName",
                LastEditorName = "LastEditorName",
                Note = "Note",
                SecondsOfWork = 90,
                WorkingDate = dateTime
            };
            Assert.Equal(detailsWtDto, result);
        }
    }
}
