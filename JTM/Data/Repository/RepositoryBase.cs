using JTM.Data;
using JTM.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JTM.Data.Repository
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntityBase, new()
    {
        private readonly DataContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task RemoveAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            query = includeProperties.Aggregate(
                query,
                (current, property) => current.Include(property));
            query.Where(c => c.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter is not null)
                query = query.Where(filter);

            query = includeProperties.Aggregate(
                query,
                (current, property) => current.Include(property));

            return await query.ToListAsync();
        }

        public async Task<TEntity?> QuerySingleAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter is not null)
                query = query.Where(filter);

            query = includeProperties.Aggregate(
                query,
                (current, property) => current.Include(property));

            return await query.FirstOrDefaultAsync();
        }

        public Task UpdateAsync(int id, TEntity entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _dbSet.AnyAsync(c => c.Id == id);
        }
    }
}
