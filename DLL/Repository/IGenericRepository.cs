using System.Linq.Expressions;

namespace DLL.Repository
{
    public interface IGenericRepository<T, in TPrimaryKey>
    {
        Task AddAsync(T entity);

        Task<T> GetAsync(TPrimaryKey id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);

        Task<bool> RemoveAsync(TPrimaryKey id);

        Task UpdateAsync(T entity);
    }
}
