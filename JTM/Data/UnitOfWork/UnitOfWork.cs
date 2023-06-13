using JTM.Data.Repository.CompanyRepo;
using JTM.Data.Repository.UserRepo;
using JTM.Data.Repository.WorkingTimeRepo;

namespace JTM.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private bool _disposed;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        private UserRepository _userRepository;
        public IUserRepository UserRepository
            => _userRepository ??= new UserRepository(_context);

        private WorkingTimeRepository _workingTimeRepository;
        public IWorkingTimeRepository WorkingTimeRepository
            => _workingTimeRepository ??= new WorkingTimeRepository(_context);

        private CompanyRepository _companyRepository;
        public ICompanyRepository CompanyRepository 
            => _companyRepository ??= new CompanyRepository(_context);

        public async ValueTask DisposeAsync()
        {
            await DisposedAsync(true);
            GC.SuppressFinalize(this);
        }

        public virtual async ValueTask DisposedAsync(bool disposing)
        {
            if (_disposed)
                return;
            if (!disposing)
                return;
            await _context.DisposeAsync();
            _disposed = true;

        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
