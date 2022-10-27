using DLL.Repository;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<City> GetAsync(int id)
        {
            return await _cityRepository.GetAsync(id);
        }
        public async Task<bool> RemoveAsync(int id)
        {
            return await _cityRepository.RemoveAsync(id);
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
