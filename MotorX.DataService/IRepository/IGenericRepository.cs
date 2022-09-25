using MotorX.DataService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> AddAsync(T enity);
        Task<IEnumerable<T>> GetAllAsync(PaginationFilter? paginationFilter = null);
        IQueryable<T> GetAll();
        Task<T?> GetAsync(Guid Key);

        Task<T?> UpdateAsync(T entity, Guid Key);

        Task<bool> DeleteAsync(Guid Key);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> match);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match, PaginationFilter? paginationFilter = null);
        Task<T?> FindAsync(Expression<Func<T, bool>> match);
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
    }
}
