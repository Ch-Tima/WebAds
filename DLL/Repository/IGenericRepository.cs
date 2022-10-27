using System.Linq.Expressions;

namespace DLL.Repository
{
    public interface IGenericRepository<T>
    {
        Task AddAsync(T entity);

        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        Task<bool> RemoveAsync(int id);

        Task UpdateAsync(T entity);
    }
}
