using DLL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public class CityRepository : IGenericRepository<City, string>
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
            return await _dbContext.Cities.Select(x => new City()
            {
                Ads = x.Ads,
                Name = x.Name,
                Region = x.Region
            }).Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _dbContext.Cities.Select(city => new City()
            {
                Name = city.Name,
                Region = city.Region,
                Ads = new List<Ad>(_dbContext.Ads.Where(ad => ad.CityName == city.Name).ToList())
            }).ToListAsync();
        }
        public async Task<City> GetAsync(string name)
        {
            return await _dbContext.Cities
                .Include(x => x.Ads)
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Name == name);
        }
        public async Task<bool> IsExists(string cityName)
        {
            return await _dbContext.Cities.AnyAsync(x => x.Name == cityName);
        }

        public async Task<bool> RemoveAsync(string name)
        {
            var city = await _dbContext.Cities.FirstOrDefaultAsync(x => x.Name == name);

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
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
