using DLL.Repository;
using Domain.Models;

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
        public async Task<bool> RemoveAsync(string name)
        {
            return await _cityRepository.RemoveAsync(name);
        }

        public async Task UpdateAsync(City entity)
        {
            try
            {
                await _cityRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
