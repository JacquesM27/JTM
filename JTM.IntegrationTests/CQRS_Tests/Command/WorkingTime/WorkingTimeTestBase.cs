using JTM.Data.UnitOfWork;
using Moq;

namespace JTM.IntegrationTests.CQRS_Tests.Command.WorkingTime
{
    public class WorkingTimeTestBase
    {
        protected readonly Mock<IUnitOfWork> MockUnitOfWork;

        public WorkingTimeTestBase()
        {
            MockUnitOfWork = new();
        }
    }
}
