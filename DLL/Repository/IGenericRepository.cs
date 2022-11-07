using Domain.Models;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public interface IGenericRepository<T, PrimaryKey>
    {
        Task AddAsync(T entity);

        Task<T> GetAsync(PrimaryKey id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);

        Task<bool> RemoveAsync(PrimaryKey id);

        Task UpdateAsync(T entity);
    }
}
