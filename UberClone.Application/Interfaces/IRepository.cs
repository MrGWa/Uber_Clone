using System.Linq.Expressions;

namespace UberClone.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(object id);
    Task<T?> GetByConditionAsync(Expression<Func<T, bool>> expression);
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetByConditionsAsync(Expression<Func<T, bool>> expression);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
}
