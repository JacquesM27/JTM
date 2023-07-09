using JTM.Data.Repository.UserRepo;
using JTM.Data.UnitOfWork;
using JTM.Services.RabbitService;
using JTM.Services.TokenService;
using Moq;

namespace JTM.UnitTests.CQRS_Tests.Command.Account
{
    public abstract class AccountTestsBase
    {
        protected readonly Mock<IUnitOfWork> MockUnitOfWork;
        protected readonly Mock<IUserRepository> MockUserRepository;
        protected readonly Mock<IBrokerService> MockRabbitService;
        protected readonly Mock<ITokenService> MockTokenService;

        protected AccountTestsBase()
        {
            MockUnitOfWork = new();
            MockUserRepository = new();
            MockRabbitService = new();
            MockTokenService = new();
        }
    }
}
