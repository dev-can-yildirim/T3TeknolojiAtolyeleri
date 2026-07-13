using System.Linq.Expressions;
using T3TeknolojiAtolyeleri.Domain.Common;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<List<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);

        Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);

        Task<List<T>> GetAllWithIncludePathsAsync(params string[] includePaths);

        Task<T?> GetByIdWithIncludePathsAsync(int id, params string[] includePaths);

        IQueryable<T> GetQueryable();

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);

        void Update(T entity);

        void SoftDelete(T entity);

        void Delete(T entity);

        Task<int> SaveChangesAsync();
    }
}
