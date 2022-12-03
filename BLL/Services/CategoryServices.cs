using Domain.Models;
using DLL.Repository;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class CategoryServices
    {
        private readonly CategoryRepository _categoryRepository;
        public CategoryServices(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task AddAsync(Category entity)
        {
            await _categoryRepository.AddAsync(entity);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }
        public async Task<IEnumerable<Category>> Find(Expression<Func<Category, bool>> expression)
        {
            return await _categoryRepository.Find(expression);
        }
        public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
        {
            return await _categoryRepository.GetMainCategoriesAsync();
        }

        public async Task<Category> GetAsync(int id)
        {
            return await _categoryRepository.GetAsync(id);
        }
        public async Task<Category> GetOnlyAsync(int id)
        {
            return await _categoryRepository.GetOnlyAsync(id);
        }

        public async Task<bool> IsExists(int id)
        {
            return await _categoryRepository.IsExists(id);
        }
        public async Task<bool> RemoveAsync(int id)
        {
            return await _categoryRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(Category entity)
        {
            await _categoryRepository.UpdateAsync(entity);
        }

    }
}
