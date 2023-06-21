using JTM.Data.Repository.UserRepo;
using JTM.Data.UnitOfWork;
using JTM.Services.RabbitService;
using Moq;

namespace JTM.UnitTests.CQRS_Tests.Command.Account
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
