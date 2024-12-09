using System.Linq.Expressions;

namespace Contracts.Interfaces.RepositoryInterfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> FindAllAsync(IEnumerable<Expression<Func<T, bool>>> criteria, int pagNumber = 1, int pageSize = 10,
              string[] includes = null);
        Task<T?> GetByIdAsync(int id);
        Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
