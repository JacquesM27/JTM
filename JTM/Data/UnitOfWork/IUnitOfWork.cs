using JTM.Data.Repository.CompanyRepo;
using JTM.Data.Repository.UserRepo;
using JTM.Data.Repository.WorkingTimeRepo;

namespace JTM.Data.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserRepository UserRepository { get; }
        IWorkingTimeRepository WorkingTimeRepository { get; }
        ICompanyRepository CompanyRepository { get; }

        Task SaveChangesAsync();
    }
}
