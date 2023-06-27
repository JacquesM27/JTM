using JTM.CQRS.Query.Company;
using JTM.DTO.Company;
using JTM.Exceptions;
using Moq;
using System.Linq.Expressions;
using Model = JTM.Data.Model;

namespace JTM.UnitTests.CQRS_Tests.Query.Company
{
    public class GetCompanyTests : UnitTestBase
    {
        [Fact]
        public async Task GetCompany_ForInvalidArgument_ShouldThrowsCompanyException()
        {
            // Arrange 
            int tmpId = -1;
            MockUnitOfWork
                .Setup(c => c.CompanyRepository.QuerySingleAsync(It.IsAny<Expression<Func<Model.Company, bool>>>()))
                .Returns(Task.FromResult<Model.Company?>(null));
            var command = new GetCompanyQuery(tmpId);
            var commandHandler = new GetCompanyQueryHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<CompanyException>(HandleCommand);
            Assert.Equal($"Company with id: {tmpId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task GetCompany_ForExistingCompany_ShouldReturnCompanyDtoObject()
        {
            // Arrange
            int tmpId = -1;
            var company = new Model.Company()
            {
                Id = tmpId,
                Name = "Test",
            };
            MockUnitOfWork
                .Setup(c => c.CompanyRepository.QuerySingleAsync(It.IsAny<Expression<Func<Model.Company, bool>>>()))
                .Returns(Task.FromResult<Model.Company?>(company));
            var command = new GetCompanyQuery(tmpId);
            var commandHandler = new GetCompanyQueryHandler(MockUnitOfWork.Object);

            // Act
            var result = await commandHandler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CompanyDto>(result);
            Assert.True(company.Id == result.Id);
        }
    }
}
