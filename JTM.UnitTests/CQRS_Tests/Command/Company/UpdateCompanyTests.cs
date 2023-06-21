using JTM.CQRS.Command.Company.UpdateCompany;
using JTM.Exceptions;
using Moq;
using Model = JTM.Data.Model;

namespace JTM.UnitTests.CQRS_Tests.Command.Company
{
    public class UpdateCompanyTests : UnitTestBase
    {
        [Fact]
        public async Task UpdateCompany_ForDiffrentRouteAndBodyIds_ShouldThrowCompanyException()
        {
            // Arrange
            var command = new UpdateCompanyCommand(string.Empty, 1, 2);
            var commandHandler = new UpdateCompanyCommandHandler(MockUnitOfWork.Object);
            async Task HandleCommand() => await commandHandler.Handle(command, default);
            
            // Act & Assert
            var exception = await Assert.ThrowsAnyAsync<CompanyException>(HandleCommand);
            Assert.Equal($"Id from header({command.HeaderId}) does not equal id from route({command.RouteId}).", exception.Message);
        }

        [Fact]
        public async Task UpdateCompany_ForNotExistCompany_ShouldThrowCompanyException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(c => c.CompanyRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Model.Company?>(null));

            // Act
            var command = new UpdateCompanyCommand(string.Empty, 1, 1);
            var commandHandler = new UpdateCompanyCommandHandler(MockUnitOfWork.Object);
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<CompanyException>(HandleCommand);
            Assert.Equal($"Company with Id:{command.RouteId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateCompany_ForValidData_ShouldPassWithoutException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(c => c.CompanyRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Model.Company?>(new Model.Company()));
            MockUnitOfWork
                .Setup(c => c.CompanyRepository.UpdateAsync(It.IsAny<int>(), It.IsAny<Model.Company>()));

            // Act
            var command = new UpdateCompanyCommand(string.Empty, 1, 1);
            var commandHandler = new UpdateCompanyCommandHandler(MockUnitOfWork.Object);
            await commandHandler.Handle(command, default);

            // Assert
        }
    }
}
