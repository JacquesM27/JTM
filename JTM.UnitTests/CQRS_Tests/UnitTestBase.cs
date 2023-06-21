using JTM.Data.UnitOfWork;
using Moq;

namespace JTM.UnitTests.CQRS_Tests
{
    public class UnitTestBase
    {
        protected readonly Mock<IUnitOfWork> MockUnitOfWork;

        public UnitTestBase()
        {
            MockUnitOfWork = new();
        }
    }
}
