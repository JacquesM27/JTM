using System.Linq.Expressions;
using JTM.Data.Model;

namespace JTM.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class, IEntityBase, new()
    {
        Task<IEnumerable<TEntity>> QueryAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity?> QuerySingleAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task UpdateAsync(int id, TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task RemoveAsync(IEnumerable<TEntity> entities);
        Task AddAsync(TEntity entity);
        Task<bool> AnyAsync(int id);
    }
}
