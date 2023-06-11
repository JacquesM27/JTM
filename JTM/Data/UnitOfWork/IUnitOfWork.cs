using JTM.Data.Repository;
using JTM.Data.Repository.UserRepo;

namespace JTM.Data.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserRepository UserRepository { get; }
        IWorkingTimeRepository WorkingTimeRepository { get; }

        Task SaveChangesAsync();
    }
}
