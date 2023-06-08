using JTM.Data.Repository;

namespace JTM.Data.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserRepository UserRepository { get; }

        Task SaveChanges();
    }
}
