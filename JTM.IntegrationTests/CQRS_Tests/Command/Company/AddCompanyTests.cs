using FluentValidation;
using JTM.CQRS.Command.Company.AddCompany;
using Moq;
using System.Linq.Expressions;
using Model = JTM.Data.Model;

namespace JTM.IntegrationTests.CQRS_Tests.Command.Company
{
    public class AddCompanyTests : CompanyTestBase
    {
        [Fact]
        public async Task AddCompany_ForBusyName_ShouldThrowValidationExceptionWithNameBusyMessage()
        {
            // Arrange
            MockUnitOfWork
                .Setup(c => c.CompanyRepository.QuerySingleAsync(It.IsAny<Expression<Func<Model.Company,bool>>>()))
                .Returns(Task.FromResult<Model.Company?>(new Model.Company()));

            var command = new AddCompanyCommand("");
            var commandHandler = new AddCompanyCommandHandler(MockUnitOfWork.Object);

            // Act
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<ValidationException>(HandleCommand);
            Assert.Equal("Company name is busy.", exception.Message);
        }

        [Fact]
        public async Task AddCompany_ForValidData_ShouldPassWithoutException()
        {
            // Arrange 
            MockUnitOfWork
                .Setup(x => x.CompanyRepository.QuerySingleAsync(It.IsAny<Expression<Func<Model.Company, bool>>>()))
                .Returns(Task.FromResult<Model.Company?>(null));
            MockUnitOfWork
                .Setup(x => x.CompanyRepository.AddAsync(It.IsAny<Model.Company>()));

            var command = new AddCompanyCommand("");
            var commandHandler = new AddCompanyCommandHandler(MockUnitOfWork.Object);

            // Act
            await commandHandler.Handle(command, default);
            // Assert
        }
    }
}
