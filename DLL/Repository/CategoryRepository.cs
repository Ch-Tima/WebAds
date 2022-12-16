using DLL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public class CategoryRepository : IGenericRepository<Category, int>
    {
        private readonly AdDbContext _dbContext;
        public CategoryRepository(AdDbContext context)
        {
            _dbContext = context;
        }
        public async Task AddAsync(Category entity)
        {
            await _dbContext.Categories.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> Find(Expression<Func<Category, bool>> expression)
        {
            return await _dbContext.Categories.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
        {
            var result = await _dbContext.Categories
                .Where(x => x.CategoryId == null)
                .ToListAsync();

            foreach (var item in result)
                item.Categories = ((List<Category>)(await UploadPropertyNavigation(item)));

            return result;
        }

        private async Task<IEnumerable<Category>> UploadPropertyNavigation(Category category)
        {
            var result = new List<Category>();

            var listCategory = await _dbContext.Categories
                .Where(x => x.CategoryId == category.Id)
                .Include(x => x.Categories).ToListAsync();

            foreach (var item in listCategory)
            {
                item.Categors = new Category();
                item.Ads = new List<Ad>();

                if (item.Categories?.Count() > 0)
                    item.Categories.ToList().AddRange(await UploadPropertyNavigation(item));

                result.Add(item);
            }

            return result;
        }

        public async Task<Category> GetAsync(int id)
        {
            return await _dbContext.Categories
                .Include(x => x.Ads)
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category> GetOnlyAsync(int id)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExists(int id)
        {
            return await _dbContext.Categories.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task UpdateAsync(Category entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
