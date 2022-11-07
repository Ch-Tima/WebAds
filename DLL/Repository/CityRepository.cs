
using DLL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public class CityRepository : IGenericRepository<City, int>
    {
        private readonly AdDbContext _dbContext;
        public CityRepository(AdDbContext context)
        {
            _dbContext = context;
        }
        public async Task AddAsync(City entity)
        {
            await _dbContext.Cities.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<City>> Find(Expression<Func<City, bool>> expression)
        {
            return await _dbContext.Cities.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _dbContext.Cities.ToListAsync();
        }
        public async Task<City> GetAsync(int id)
        {
            return await _dbContext.Cities.Include(x => x.Ads).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<bool> RemoveAsync(int id)
        {
            var city = await _dbContext.Cities.FirstOrDefaultAsync(x => x.Id == id);

            if (city != null)
            {
                _dbContext.Cities.Remove(city);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task UpdateAsync(City entity)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
