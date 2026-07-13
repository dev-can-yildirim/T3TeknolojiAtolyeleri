using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using T3TeknolojiAtolyeleri.Domain.Common;
using T3TeknolojiAtolyeleri.Persistance.Context;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;

namespace T3TeknolojiAtolyeleri.Persistance.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _dbSet.Update(entity);
        }

        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.Now;
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<List<T>> GetAllWithIncludePathsAsync(params string[] includePaths)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includePath in includePaths)
            {
                query = query.Include(includePath);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdWithIncludePathsAsync(int id, params string[] includePaths)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includePath in includePaths)
            {
                query = query.Include(includePath);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
