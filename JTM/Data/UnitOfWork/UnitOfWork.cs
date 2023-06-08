using JTM.Data.Repository;

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
        {
            get
            {
                _userRepository ??= new UserRepository(_context);
                return _userRepository;
            }
        }

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
