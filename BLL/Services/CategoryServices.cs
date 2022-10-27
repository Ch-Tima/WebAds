using DLL.Repository;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Category> GetAsync(int id)
        {
            return await _categoryRepository.GetAsync(id);
        }
        public async Task<bool> RemoveAsync(int id)
        {
            return await _categoryRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(Category entity)
        {
            try
            {
                await _categoryRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
