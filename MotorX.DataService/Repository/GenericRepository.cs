using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MotorX.DataService.Data;
using MotorX.DataService.Entities;
using MotorX.DataService.IRepository;
using System.Linq.Expressions;

namespace MotorX.DataService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _context;
        protected ILogger _logger;
        internal DbSet<T> dbset;
        public GenericRepository(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            dbset = context.Set<T>();
        }

        public virtual async Task<T?> AddAsync(T enity)
        {
            var result = await dbset.AddAsync(enity);
            if (result is null)
            {
                return null;
            }
            return enity;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(PaginationFilter? paginationFilter = null)
        {
            return await dbset.AsNoTracking().ToListAsync();
        }
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }
        public virtual async Task<bool> DeleteAsync(Guid Key)
        {
            // hard delete
            var entity = await dbset.FindAsync(Key);
            if (entity is not null)
            {
                dbset.Remove(entity);
                return true;
            }
            return false;
        }

        public virtual async Task<T?> GetAsync(Guid Key)
        {
            return await dbset.FindAsync(Key);
        }

        public virtual async Task<T?> UpdateAsync(T entity, Guid Key)
        {
            if (entity is null)
                return null;

            T? existing = await dbset.FindAsync(Key);

            if (existing is not null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
            }

            return entity;
        }

        public virtual async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().CountAsync(match);
        }

        public virtual async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match, PaginationFilter? paginationFilter = null)
        {
            return await _context.Set<T>().Where(match).AsNoTracking().ToListAsync();
        }
        public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(match);
        }
        public virtual async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }
        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable.AsNoTracking();
        }
    }
}
