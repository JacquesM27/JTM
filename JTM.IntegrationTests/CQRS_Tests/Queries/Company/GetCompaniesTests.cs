using JTM.CQRS.Query.Company;
using JTM.DTO.Company;
using Moq;
using Model = JTM.Data.Model;

namespace JTM.IntegrationTests.CQRS_Tests.Queries.Company
{
    public class GetCompaniesTests : CompanyTestBase
    {
        [Fact]
        public async Task GetCompanies_ForNoneCompany_ShouldReturnEmptyCollection()
        {
            // Arrange
            MockUnitOfWork
                .Setup(c => c.CompanyRepository.QueryAsync(null))
                .ReturnsAsync(new List<Model.Company>());
            var command = new GetCompaniesQuery();
            var commandHandler = new GetCompaniesQueryHandler(MockUnitOfWork.Object);

            // Act
            var result = await commandHandler.Handle(command, default);

            // Assert
            Assert.Empty(result);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<CompanyDto>>(result);
        }

        [Fact]
        public async Task GetCompanies_ForTwoCompaniesWithDetails_ShouldReturnIEnumerableWithTwoElements()
        {
            // Arrange
            var companies = new List<Model.Company>
            {
                new Model.Company { Id = 1, Name = "Test" },
                new Model.Company { Id = 2, Name = "Test2" }
            };
            MockUnitOfWork
                .Setup(c => c.CompanyRepository.QueryAsync(null))
                .ReturnsAsync(companies);
            var command = new GetCompaniesQuery();
            var commandHandler = new GetCompaniesQueryHandler(MockUnitOfWork.Object);

            // Act
            var result = await commandHandler.Handle(command, default);

            // Assert
            Assert.NotEmpty(result);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<CompanyDto>>(result);
        }
    }
}
