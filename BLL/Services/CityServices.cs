using DLL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class CityServices
    {
        private readonly CityRepository _cityRepository;
        public CityServices(CityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public async Task AddAsync(City entity)
        {
            await _cityRepository.AddAsync(entity);
        }
        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _cityRepository.GetAllAsync();
        }
        public async Task<City> GetAsync(string name)
        {
            return await _cityRepository.GetAsync(name);
        }

        public async Task<IEnumerable<City>> Find(Expression<Func<City, bool>> expression)
        {
            return await _cityRepository.Find(expression);
        }

        public async Task<bool> IsExists(string cityName)
        {
            return await _cityRepository.IsExists(cityName);
        }
        public async Task<bool> RemoveAsync(string name)
        {
            return await _cityRepository.RemoveAsync(name);
        }

        public async Task UpdateAsync(City entity)
        {
            await _cityRepository.UpdateAsync(entity);
        }
    }
}
