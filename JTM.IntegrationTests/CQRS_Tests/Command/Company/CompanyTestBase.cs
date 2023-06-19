using JTM.Data.UnitOfWork;
using Moq;

namespace JTM.IntegrationTests.CQRS_Tests.Command.Company
{
    public class CompanyTestBase
    {
        protected readonly Mock<IUnitOfWork> MockUnitOfWork;

        public CompanyTestBase()
        {
            MockUnitOfWork = new();
        }
    }
}
