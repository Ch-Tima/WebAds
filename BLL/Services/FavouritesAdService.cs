using DLL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class FavouritesAdService
    {
        private readonly FavouritesAdRepository _favouritesAd;

        public FavouritesAdService(FavouritesAdRepository favouritesAd)
        {
            _favouritesAd = favouritesAd;
        }
        public async Task<bool> AddAsync(string userId, int adId)
        {
            try
            {
                if (await this.AnyAsync(x => x.AdId == adId && x.UserId == userId))
                    return false;

                await _favouritesAd.AddAsync(new FavouritesAd()
                {
                    UserId = userId,
                    AdId = adId
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<FavouritesAd>> Find(Expression<Func<FavouritesAd, bool>> expression)
        {
            return await _favouritesAd.Find(expression);
        }

        public async Task<IEnumerable<FavouritesAd>> GetAllAsync()
        {
            return await _favouritesAd.GetAllAsync();
        }

        public async Task<FavouritesAd> GetAsync(int id)
        {
            return await _favouritesAd.GetAsync(id);
        }

        public async Task<bool> RemoveAsync(string userId, int adId)
        {
            var favourites = await _favouritesAd.Find(x => x.AdId == adId && x.UserId == userId);

            if (favourites == null || favourites.Count() == 0)
                return false;

            return await _favouritesAd.RemoveAsync(favourites.First().Id);
        }
        public async Task<bool> AnyAsync(Expression<Func<FavouritesAd, bool>> expression)
        {
            return await _favouritesAd.AnyAsync(expression);
        }
    }
}
