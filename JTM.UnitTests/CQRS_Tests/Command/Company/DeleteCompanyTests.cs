using JTM.CQRS.Command.Company.DeleteCompany;
using JTM.Exceptions;
using Moq;
using Model = JTM.Data.Model;

namespace JTM.UnitTests.CQRS_Tests.Command.Company
{
    public class DeleteCompanyTests : UnitTestBase
    {
        [Fact]
        public async Task DeleteCompany_ForNotExistingCompany_ShouldThrowNotExistException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(c => c.CompanyRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Model.Company?>(null));
            int dummyId = -1;
            
            // Act
            var command = new DeleteCompanyCommand(dummyId);
            var commandHandler = new DeleteCompanyCommandHandler(MockUnitOfWork.Object);
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<CompanyException>(HandleCommand);
            Assert.Equal($"Company with id:{dummyId} does not exist.", exception.Message);
        }

        [Fact]
        public async Task DeleteCompany_ForValidData_ShouldPassWithoutException()
        {
            // Arrange
            MockUnitOfWork
                .Setup(c => c.CompanyRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Model.Company?>(new Model.Company()));
            MockUnitOfWork
                .Setup(c => c.CompanyRepository.RemoveAsync(It.IsAny<Model.Company>()));

            // Act
            var command = new DeleteCompanyCommand(It.IsAny<int>());
            var commandHandler = new DeleteCompanyCommandHandler(MockUnitOfWork.Object);
            await commandHandler.Handle(command, default);

            // Assert
        }
    }
}
