using JTM.Data.Repository;
using JTM.Data.UnitOfWork;
using JTM.Services.RabbitService;
using Moq;

namespace JTM.IntegrationTests.CQRS_Tests.Command.Account
{
    public abstract class AccountTestsBase
    {
        protected readonly Mock<IUnitOfWork> MockUnitOfWork;
        protected readonly Mock<IUserRepository> MockUserRepository;
        protected readonly Mock<IRabbitService> MockRabbitService;

        protected AccountTestsBase()
        {
            MockUnitOfWork = new();
            MockUserRepository = new();
            MockRabbitService = new();
        }
    }
}
