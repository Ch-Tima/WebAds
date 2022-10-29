
using DLL.Repository;
using Domain.Models;

namespace BLL.Services
{
    public class AdsServices
    {
        private readonly AdRepository _AdRepository;
        public AdsServices(AdRepository AdRepository)
        {
            _AdRepository = AdRepository;
        }
        public async Task AddAsync(Ad entity, int cityId, int categoryId)
        {
            entity.CityId = cityId;
            entity.CategotyId = categoryId;

            await _AdRepository.AddAsync(entity);
        }
        public async Task<IEnumerable<Ad>> GetAllAsync()
        {
            return await _AdRepository.GetAllAsync();
        }
        public async Task<IEnumerable<Ad>> Find(string userId)
        {
            var t = await _AdRepository.Find(x => x.UserId == userId);
            return t;
        }
        public async Task<Ad> GetAsync(int id, bool fullLoad = false)
        {
            if (fullLoad)
                return await _AdRepository.GetAsync(id);
            else
                return await _AdRepository.GetOnlyAsync(id);

        }
        public async Task<bool> RemoveAsync(int id)
        {
            return await _AdRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(Ad entity)
        {
            try
            {
                await _AdRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
