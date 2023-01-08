using DLL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public class FavouritesAdRepository : IGenericRepository<FavouritesAd, int>
    {

        private readonly AdDbContext _dbContext;
        public FavouritesAdRepository(AdDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(FavouritesAd entity)
        {
            try
            {
                await _dbContext.FavouritesAds.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<FavouritesAd>> Find(Expression<Func<FavouritesAd, bool>> expression)
        {
            return await _dbContext.FavouritesAds.Where(expression)
                            .Include(x => x.Ad)
                            .Include(x => x.User)
                            .Select(x => new FavouritesAd()
                            {
                                Id = x.Id,
                                AdId = x.AdId,
                                Ad = new Ad()
                                {
                                    Id = x.Ad.Id,
                                    Name = x.Ad.Name,
                                    CategotyId = x.Ad.CategotyId,
                                    CityName = x.Ad.CityName,
                                    IsVerified = x.Ad.IsVerified,
                                    IsTop = x.Ad.IsTop,
                                    PathImg = x.Ad.PathImg,
                                    Price = x.Ad.Price,
                                    DateCreate = x.Ad.DateCreate,
                                    Content = x.Ad.Content,
                                    UserId = x.Ad.UserId

                                },
                                UserId = x.UserId,
                                User = new User()
                                {
                                    Id = x.UserId,
                                    UserName = x.User.UserName,
                                    Surname = x.User.Surname,
                                    Email = x.User.Email,
                                    CityName = x.Ad.CityName,
                                }
                            })
                            .ToListAsync();
        }

        public async Task<IEnumerable<FavouritesAd>> GetAllAsync()
        {
            return await _dbContext.FavouritesAds.ToListAsync();
        }

        public async Task<FavouritesAd> GetAsync(int id)
        {
            return await _dbContext.FavouritesAds
                .Include(x => x.User)
                .Include(x => x.Ad)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                _dbContext.FavouritesAds.Remove(await _dbContext.FavouritesAds.FirstOrDefaultAsync(x => x.Id == id));
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<FavouritesAd, bool>> expression)
        {
            return await _dbContext.FavouritesAds.AnyAsync(expression);
        }

        public Task UpdateAsync(FavouritesAd entity)
        {
            throw new NotImplementedException();
        }
    }
}
