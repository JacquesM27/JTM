using JTM.Data.Repository;
using JTM.Data.UnitOfWork;
using JTM.Services.RabbitService;
using Moq;

namespace JTM.IntegrationTests.CQRS.Command.Account
{
    public abstract class AccountTestsBase
    {
        protected readonly Mock<IUnitOfWork> _mockUnitOfWork;
        protected readonly Mock<IUserRepository> _mockUserRepository;
        protected readonly Mock<IRabbitService> _mockRabbitService;

        protected AccountTestsBase()
        {
            _mockUnitOfWork = new();
            _mockUserRepository = new();
            _mockRabbitService = new();
        }
    }
}
